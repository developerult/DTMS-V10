<%@ Page Title="Rate Shopping Page" Language="C#"  AutoEventWireup="true" CodeBehind="RateShopping.aspx.cs" Inherits="DynamicsTMS365.RateShopping" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Rate Shopping</title>          
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
                        <div id="order-pane">
                            <div class="pane-content" >
                                <%--style="margin-left:20px; margin-right:10px; margin-bottom:2px; margin-top:2px;"
                                    style="border-collapse:collapse; border-spacing:0; border:none; margin-top:5px; margin-bottom:5px; width:auto;"
                                --%>
                                <div class="k-block k-info-colored" style=" margin-left:2px; margin-right:2px; margin-top:2px; width:auto; padding-left:2px; padding-right:2px;" >
                                    <label><b>Current Load To Rate</b></label>
                                    <table id="tordersummary" style="border-collapse:collapse; border-spacing:0; border:none; margin-top:0px; margin-bottom:0px; width:100%;">
                                        <tr style="border: none;">
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Origin</th>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Destination</th>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Summary</th>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Wgt</th>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Qty</th>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Pkgs</th>
                                           <%-- Modified by RHR for v-8.5.4.001 add temperature--%>
                                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Temp</th>
                                        </tr>
                                        <tr style="border: none;">
                                            <td style="border: none; width: 30%; min-width: 100px; padding-right:4px;"><span id="spOrigin"></span></td>
                                            <td style="border: none;  width: 30%; min-width: 100px; padding-right:4px;"><span id="spDest"></span></td>
                                            <td style="border: none;  width: 25%; min-width: 100px; padding-right:4px;"><span id="spShipDetails"></span></td>
                                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="spShipWgt"></span></td>
                                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="spShipQty"></span></td>
                                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="spShipPlts"></span></td>
                                             <%-- Modified by RHR for v-8.5.4.001 add temperature--%>
                                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="spShipTemperature"></span></td>
                                        </tr>
                                        <%--<tr style="border: none;"><td colspan="3" style="border-top: none; border-left: none; border-right: none;"></td></tr>--%>
                                    </table>                                                                     
                                </div>  
                                <%--<div>                                   
                                     <br />                                
                                    <a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" href="#" onclick="openNGLErrWarnMsgLogCtrlDialog()">
                                        <span class="k-icon k-i-window" style="color:blue;" ></span>&nbsp;Show Messages
                                    </a>
                                    <br />  
                                </div> --%>
                                <!-- begin Page Content -->
                                <% Response.Write(FastTabsHTML); %>    
                                <!-- End Page Content -->
                                             &nbsp;                       
                                <div id="RatesDiv" class="OpenOrders">             
                                    <div id="carriersGrid"></div>
                                </div> 
                                                   
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
  
                  
          <div id="zipWin"> 
              <div>
                  <input class="zipWin" id="comboZip" />
              </div>
              <div>
                  <button class="zipWin" type="button" id="btnZipOk" onclick="btnZipOk_Click();">Ok</button>
              </div>
              <input id="txtZipCity" type="hidden" />
              <input id="txtZipState" type="hidden" />
              <input id="txtZipLocation" type="hidden" />
          </div>
    
          <div id="winItems"> 
              <h2>Click to Get Rates &nbsp;&nbsp;<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" href="#" onclick="CloseWinItemAndCalculate();""><span class="k-icon k-i-download" ></span></a>&nbsp;&nbsp;<button id='btnAddressBookPopup' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddressBook, 145);'><span class='k-icon k-i-book'></span>Open Address Book</button></h2>
            <div class="fast-tab" >                
                
               <table id="tblShippingInfo" style="border-collapse:collapse; border-spacing:0; border:none; margin-top:5px;">
                   <tr>
                        <td style="border:none;">Ship Date: </td><td style="border:none;"><input id="txtShipDate" style="width: 150px;" /></td>
                        <td style="border:none;">Req Date: </td><td style="border:none;"><input id="txtDeliveryDate" style="width: 150px;" /></td>
                    </tr>
                    <tr>
                     <td style="border:none;">Orig Postal Code: </td><td style="border:none;"><input id="txtorigCompPostalCode" style="width: 150px;" /></td>
                     <td style="border:none;">Dest Postal Code: </td><td style="border:none;"><input id="txtdestCompPostalCode" style="width: 150px;" /></td>
                   </tr>
               </table> 
               <table id="tblOrderInfo" style="border-collapse:collapse; border-spacing:0; border:none; margin-top:5px;">
                 <tr>                           
                     <th style="border:none;">SHID</th> 
                     <th style="border:none;">Weight UOM</th> 
                     <th style="border:none;">Length UOM</th>                                            
                     <th style="border:none;">Total Wgt</th> 
                     <th style="border:none;">Total Qty</th>
                     <th style="border:none;">Total Pkgs</th>  
                     <th style="border:none;">Temperature</th> 
                     <th style="border:none;">Class</th>  
                 </tr>
                 <tr>
                     <td style="border:none;"><input id="txtSHID" style="width: 100px;" value="(Auto)" /></td>
                     <td style="border:none;"><input id="txtWeightUnit" style="width: 100px;" /></td>
                     <td style="border:none;"><input id="txtLengthUnit" style="width: 100px;" /></td>
                     <td style="border:none;"><input id="txtTotalWgt" style="width: 100px;" /></td>
                     <td style="border:none;"><input id="txtTotalCases" style="width: 100px;" /></td>
                     <td style="border:none;"><input id="txtTotalPlts" style="width: 100px;" /></td>
                     <td style="border:none;"><input id="ddlLoadTemp" style="width: 100px;" /></td>   
                     <td style="border:none;"><input id="txtClass" style="width: 100px;" /></td>
                 </tr>
             </table> 
                <span id="ExpandOrderInfoSpan" style="display: none;"><a onclick='expandOrderInfo();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                <span id="CollapseOrderInfoSpan" style="display: normal;"><a onclick='collapseOrderInfo();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                         
                <span style="font-size:small; font-weight:bold">Address Details</span><br/>
                <div id="divOrderInfo" class="fast-tab" style="display:normal;">                                        
                    <table id="tblOrigInfo" style="border-collapse:collapse; border-spacing:0; border:none; margin-top:5px;"> 
                        <tr>    
                            <th style="border:none;">Origin Location</th>                             
                            <th style="border:none;">Street</th>
                            <th style="border:none;">City</th>
                            <th style="border:none;">State</th>
                            <th style="border:none;">Country</th>                                             
                        </tr>
                        <tr>
                            <td style="border:none;"><input id="txtorigCompName" style="width: 200px;" value="Ship From" /></td>
                            <td style="border:none;"><input id="txtorigCompAddress1" style="width: 200px;" /></td>
                            <td style="border:none;"><input id="txtorigCompCity" style="width: 200px;" /></td>
                            <td style="border:none;"><input id="txtorigCompState" style="width: 150px;" /></td>
                            <td style="border:none;"><input id="txtorigCompCountry" style="width: 100px;" /></td>
                        </tr>
                    </table>                                    
                    <table id="tblDestInfo" style="border-collapse:collapse; border-spacing:0; border:none; margin-bottom:5px; margin-top:5px;">
                        <tr>
                            <th style="border:none;">Destination Location</th>
                            <th style="border:none;">Street</th>
                            <th style="border:none;">City</th>
                            <th style="border:none;">State</th>
                            <th style="border:none;">Country</th>                                             
                        </tr>
                        <tr>
                            <td style="border:none;"><input id="txtdestCompName" style="width: 200px;" value="Ship To" /></td>
                            <td style="border:none;"><input id="txtdestCompAddress1" style="width: 200px;" /></td>
                            <td style="border:none;"><input id="txtdestCompCity" style="width: 200px;" /></td>
                            <td style="border:none;"><input id="txtdestCompState" style="width: 150px;" /></td>
                            <td style="border:none;"><input id="txtdestCompCountry" style="width: 100px;" /></td>
                        </tr>
                    </table> 

                    <input id="txtShipKey" type="hidden" />
                    <input id="txtBookConsPrefix" type="hidden" />
                    <input id="txtBookProNumber" type="hidden" />
                    <input id="txtBookControl" type="hidden" />
                    <input id="txtOrderID" type="hidden" />
                    <input id="txtBookCustCompControl" type="hidden" /> 
                    <input id="txtCompNumber" type="hidden" />  
                    <input id="txtCompAlphaCode" type="hidden" />
                    <input id="txtBookCarrierControl" type="hidden" /> 
                    <input id="txtCarrierName" type="hidden" /> 
                    <input id="txtCarrierNumber" type="hidden" />  
                    <input id="txtCarrierAlphaCode" type="hidden" /> 
                    <input id="txtBookSHID" type="hidden" /> 
                    <input id="txtorigCompControl" type="hidden" /> 
                    <input id="txtorigCompNumber" type="hidden" />  
                    <input id="txtorigCompAddress2" type="hidden" />  
                    <input id="txtorigCompAddress3" type="hidden" />
                    <input id="txtdestCompControl" type="hidden" /> 
                    <input id="txtdestCompNumber" type="hidden" />  
                    <input id="txtdestCompAddress2" type="hidden" />  
                    <input id="txtdestCompAddress3" type="hidden" />
                    <input id="txtStopIndex" type="hidden" />                                
                    <input id="txtSelectedStopIndex" type="hidden" />
                    <input id="txtItemIndex" type="hidden" />
                    <input id="txtLoadID" type="hidden" />
                    <input id="txtItemStopIndex" type="hidden" />
                    <input id="txtNumPieces" type="hidden" />
                    <input id="txtDescription" type="hidden" />
                    <input id="txtHazmatId" type="hidden" />                                    
                    <input id="txtCode" type="hidden" />
                    <input id="txtHazmatClass" type="hidden" />
                    <input id="txtIsHazmat" type="hidden" />
                    <input id="txtPieces" type="hidden" />
                    <input id="txtPackageType" type="hidden" />
                    <input id="txtDensity" type="hidden" />
                    <input id="txtNMFCItem" type="hidden" />
                    <input id="txNMFCSub" type="hidden" />
                    <input id="txtFeeIndex" type="hidden" />

                    <input id="txtOrigPhone" type="hidden" />
                    <input id="txtOrigContactName" type="hidden" />
                    <input id="txtOrigContactEmail" type="hidden" />
                    <input id="txtOrigEmergencyContactPhone" type="hidden" />
                    <input id="txtOrigEmergencyContactName" type="hidden" />

                    <input id="txtDestPhone" type="hidden" />
                    <input id="txtDestContactName" type="hidden" />
                    <input id="txtDestContactEmail" type="hidden" />
                    <input id="txtDestEmergencyContactPhone" type="hidden" />
                    <input id="txtDestEmergencyContactName" type="hidden" />

                    
                </div>
            </div>
                       
            <p></p>                         
            <div class="fast-tab" >
                <span id="ExpandCurrentItemsSpan" style="display: normal;"><a onclick='expandCurrentItems();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                <span id="CollapseCurrentItemsSpan" style="display: none;"><a onclick='collapseCurrentItems();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                         
                <span style="font-size:small; font-weight:bold">Item details</span>&nbsp;&nbsp;<br/>
                <div id="divCurrentItems" class="fast-tab" style="display:none;"> 
                         
                    <div id="Items" style="margin-top:5px;"></div>
                </div>
            </div>       
            <p></p>    
            <div class="fast-tab" >
                <span id="ExpandAccessorialsSpan" style="display: normal;"><a onclick='expandAccessorials();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                <span id="CollapseAccessorialsSpan" style="display: none;"><a onclick='collapseAccessorials();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                         
                <span style="font-size:small; font-weight:bold">Charges</span>&nbsp;&nbsp;<br/>
                <div id="divAccessorials" class="fast-tab" style="display:none;"> 
                    <div id="Accessorials" style="margin-top:5px;"></div>
                </div>
            </div>
            <p></p>   
            <div class="fast-tab" >
                 <span id="ExpandPopUpSummarySpan" style="display: none;"><a onclick='expandPopUpSummary();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                 <span id="CollapsePopUpSummarySpan" style="display: normal;"><a onclick='collapsePopUpSummary();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                         
                 <span style="font-size:small; font-weight:bold">Summary</span>&nbsp;&nbsp;<br/>
                 <div id="divPopUpSummary" class="fast-tab" style="display:normal;"> 
                    <table id="tpopupordersummary" style="border-collapse:collapse; border-spacing:0; border:none; margin-top:0px; margin-bottom:0px; width:100%;">
                        <tr style="border: none;">
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Origin</th>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Destination</th>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Dates</th>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Wgt</th>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Qty</th>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Pkgs</th>
                            <%-- Modified by RHR for v-8.5.4.001 add temperature--%>
                            <th style="border-top: none; border-left: none; border-right: none; text-align: left;">Temp</th>
                        </tr>
                        <tr style="border: none;">
                            <td style="border: none; width: 30%; min-width: 100px; padding-right:4px;"><span id="sppopupOrigin"></span></td>
                            <td style="border: none;  width: 30%; min-width: 100px; padding-right:4px;"><span id="sppopupDest"></span></td>
                            <td style="border: none;  width: 25%; min-width: 100px; padding-right:4px;"><span id="sppopupShipDetails"></span></td>
                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="sppopupShipWgt"></span></td>
                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="sppopupShipQty"></span></td>
                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="sppopupShipPlts"></span></td>
                            <%-- Modified by RHR for v-8.5.4.001 add temperature--%>
                            <td style="border: none;  width: 5%; min-width: 25px; padding-right:4px;"><span id="sppopupShipTemperature"></span></td>
                        </tr>
                        <%--<tr style="border: none;"><td colspan="3" style="border-top: none; border-left: none; border-right: none;"></td></tr>--%>
                    </table>  
                </div>
             </div>   
          </div>      

          <div id="winAddressBook">             
              <div>
                  <div style="margin-bottom:5px;">
                      <span>
                          <input id="acFilter" style="width:200px"/>
                          &nbsp;
                          <button id='btnSetOrigin' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='btnSetOrigin_Click();'><span class='k-icon k-i-map-marker-target'></span>Set Origin</button>
                          &nbsp;
                          <button id='btnSetDest' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='btnSetDest_Click();'><span class='k-icon k-i-map-marker'></span>Set Destination</button>
                      </span>               
                  </div>
                  <div id="AddressBookGrid"></div>
              </div>
          </div>

          <div id="dividEditPackagesPopupWnd"></div>
          <div id="dividAccessorialsPopupWnd"></div>

       <% Response.WriteFile("~/Views/DispatchingDialog.html"); %> 
       <% Response.WriteFile("~/Views/DispatchReport.html"); %>
       <% Response.WriteFile("~/Views/BOLReport.html"); %> 

    <script type="text/x-kendo-template" id="rateQuoteDetailstemplate">
        <div class="tabstrip">
            <ul>
                <li class="k-active">
                    Adjustments
                </li>
                <li>
                    Errors
                </li>
            </ul>            
            <div>
                <div class="adjustments"></div>
            </div>
            <div>
                <div class="rateerrors"></div>
            </div>
        </div>
    </script>

    <script type="text/x-kendo-template" id="AccGridToolbarTemplate">      
            <input id="comboAccessorials" style="width: 275px"/>
            <a class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='btnAddFee_Click();'><span class='k-icon k-i-plus'></span> Add Accessorial</a>
            <a class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='clearAccessorials_Click();'><span class='k-icon k-i-minus'></span> Clear Accessorials</a>
    </script>

    <% Response.Write(PageTemplates); %>
         
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>  
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>                  
    <% Response.WriteFile("~/Views/ImportRateShopLoadsWindow.html"); %>

    <script>
        //debugger;
       <% Response.Write(ADALPropertiesjs); %>
      
        var PageControl = '<%=PageControl%>';  
        var tObj = this;
        var tPage = this;           

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>

       

        //********************** Start Modify by RHR for v-8.4.0.002 on  05/07/2021  
        var iPltWidth = <% Response.Write(RatingDefaultPltWidth);  %>;
        var iPltHeight = <% Response.Write(RatingDefaultPltHeight);  %>;
        var iPltLength = <% Response.Write(RatingDefaultPltLength);  %>;
        var sPkgType = '<% Response.Write(RatingDefaultPkgType);  %>';
        var sFreightClass = '<% Response.Write(RatingDefaultFreightClass);  %>';
        var dWeight = <% Response.Write(RatingDefaultWeight);  %>;
        var sWeightUnit = '<% Response.Write(RatingDefaultWeightUnit);  %>';
        var sLengthUnit = '<% Response.Write(RatingDefaultLengthUnit);  %>';
        var sCountry = '<% Response.Write( DefaultCountryCode );  %>';
        var sHistQuotesFilterFuncionName = null; // will hold "obj" + sHistQuotesGridKey + "Filters"
        var sHistQuotesFilterData = null; // will hold  window[sfilterFuncionname].data;        
        var sBidLoadTenderControlVal = "0"

      

        function getExportBidsDataSource(sFilter) {       
            return new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/HistoricalQuotes/GetExportBids",
                        data: { filter: JSON.stringify(sFilter) },
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        type: "Get",
                        success: function (data) {
                            options.success(data);
                        }
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "BidControl",
                        fields: {
                            "BidCarrierName": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookDateLoad": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookOrigCity": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookOrigZip": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookDestCity": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookDestZip": { type: "string", visible: true, editable: false, nullable: true },
                            "LTBookTotalPL": { type: "number", visible: true, editable: false, nullable: true },
                            "LTBookTotalWgt": { type: "number", visible: true, editable: false, nullable: true },
                            "LTCommCodeDescription": { type: "string", visible: true, editable: false, nullable: true },
                            "BidCustTotalCost": { type: "number", visible: true, editable: false, nullable: true },
                            "BidCustLineHaul": { type: "number", visible: true, editable: false, nullable: true },
                            "BidFuelTotal": { type: "number", visible: true, editable: false, nullable: true },
                            "Fees": { type: "number", visible: true, editable: false, nullable: true },
                            "BidQuoteDate": { type: "string", visible: true, editable: false, nullable: true },
                            "BidQuoteNumber": { type: "string", visible: true, editable: false, nullable: true },
                            "FreightClass": { type: "string", visible: true, editable: false, nullable: true }
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
            });
        }
        //sBidLoadTenderControlVal
        //dsExportQuotes  sBidLoadTenderControlVal cannot be used like this url is not dynamic

        function getdsExportQuotesDataSource(sFilter) { 
        return  new kendo.data.DataSource({
            transport: {
                read: {
                    url: "api/tblBid/GetExportBids", 
                    data: { filter: sFilter },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: "Get",
                    success: function (data) {
                        //if (ngl.coreSuccessCallBack(data, "Read Quote to Export") == true) {
                            options.success(data);
                        //}
                        //alert("Success!");
                        //console.log(data);
                        //if (data.Errors != null) {
                        //    if (data.StatusCode === 203) { ngl.showErrMsg("Authorization Timeout", data.Errors, null); }
                        //    else { ngl.showErrMsg("Read Quote to Export Failure", data.Errors, null); }
                        //} else {
                        //    options.success(data);
                        //}
                       
                    },
                    error: function (xhr, textStatus, error) {
                        alert("fail!");
                        console.log(error);
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Quote to Export Failure");
                     
                        ngl.showErrMsg("Read Data", sMsg, null);
                    }

                },
            },
            schema: {
                data: "Data",
                total: "Count",
                model: {
                    id: "BidControl",
                    fields: {
                        "BidCarrierName": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookDateLoad": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookOrigCity": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookOrigZip": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookDestCity": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookDestZip": { type: "string", visible: true, editable: false, nullable: true },
                        "LTBookTotalPL": { type: "number", visible: true, editable: false, nullable: true },
                        "LTBookTotalWgt": { type: "number", visible: true, editable: false, nullable: true },
                        "LTCommCodeDescription": { type: "string", visible: true, editable: false, nullable: true },
                        "BidCustTotalCost": { type: "number", visible: true, editable: false, nullable: true },
                        "BidCustLineHaul": { type: "number", visible: true, editable: false, nullable: true },
                        "BidFuelTotal": { type: "number", visible: true, editable: false, nullable: true },
                        "Fees": { type: "number", visible: true, editable: false, nullable: true },
                        "BidQuoteDate": { type: "string", visible: true, editable: false, nullable: true },
                        "BidQuoteNumber": { type: "string", visible: true, editable: false, nullable: true },
                        "FreightClass": { type: "string", visible: true, editable: false, nullable: true }
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
        });
        }
        function updatePackagesOnChange(weight,qty,plt,fakClass){
            if (typeof (weight) !== 'undefined')  {
                weight = ngl.nbrTryParse(weight,dWeight);               
            } else {
                weight =isNull(parseInt($("#txtTotalWgt").data("kendoNumericTextBox").value()),dWeight); 
            }
            if (typeof (qty) !== 'undefined')  {
                qty = ngl.nbrTryParse(qty,1);   
            
            } else {
                qty = isNull(parseInt($("#txtTotalCases").data("kendoNumericTextBox").value()),1); 

            }
            if (typeof (plt) !== 'undefined')  {
                plt = ngl.nbrTryParse(plt,1);               
            } else {                  
                plt = isNull(parseInt($("#txtTotalPlts").data("kendoNumericTextBox").value()),1);
            }
            var blnChangeFAK = false;
            if (typeof (fakClass) !== 'undefined') {
                if (ngl.isNullOrWhitespace(fakClass) === false) {
                    blnChangeFAK = true;
                }
            } else {
                blnChangeFAK = false;
                fakClass = sFreightClass;
            }
            var newItems = new Array();
            //create a new package record for each pallet
            var ipltWgt = weight/plt;
            var ipltqty = qty/plt;
            if (ipltWgt < 1){ ipltWgt = 1;}
            if (ipltqty < 1) {ipltqty = 1;}
            for (iplt = 0; iplt < plt; ++iplt){ 
                var item = new rateReqItem();
                if(orderitems != null && orderitems.length > iplt){                    
                    var i = orderitems[iplt];                    
                    item.ItemNumber = i.ItemNumber;
                    item.Description = i.Description;
                    if (blnChangeFAK === true) {
                        item.FreightClass = fakClass;
                    } else {
                        item.FreightClass = i.FreightClass;
                    }
                    item.Weight = ngl.nbrTryParse(ipltWgt,1);
                    item.PackageType = i.PackageType;
                    item.PalletCount = 1 ;
                    item.Quantity = ngl.nbrTryParse(ipltqty,1); 
                    item.Stackable = i.Stackable;
                    item.Length = i.Length;
                    item.Width = i.Width;
                    item.Height = i.Height;
                    item.NMFCItem = i.NMFCItem; 
                    item.NMFCSub = i.NMFCSub;                                      
                }   else {
                    item.ItemNumber = (iplt + 1).toString();  
                    item.Description = ("Item " + (iplt + 1).toString()); 
                    item.FreightClass = fakClass;
                    item.Weight = ngl.nbrTryParse(ipltWgt,dWeight);
                    item.PackageType = sPkgType;
                    item.PalletCount = 1 ;
                    item.Quantity = ngl.nbrTryParse(ipltqty,1).toString(); 
                    item.Stackable = true;
                    item.Length = iPltLength;
                    item.Width = iPltWidth;
                    item.Height = iPltHeight;
                    item.NMFCItem = ""; 
                    item.NMFCSub = "";                      
                }
                newItems.push(item); 
            }
            orderitems = newItems;
            intItemNumIndex = orderitems.length;
            if (orderitems.length > 0) {
                for (index = 0; index < orderitems.length; ++index) {
                    var item = orderitems[index];
                    TotalCases += parseInt(item.Quantity);
                    TotalWgt += parseFloat(item.Weight);
                    TotalPlts += parseFloat(item.PalletCount);
                }
            }
            else {
                TotalCases = 1;
                TotalWgt = 1;
                TotalPlts = 1;
            }
            $("#txtTotalWgt").data("kendoNumericTextBox").value(TotalWgt);
            $("#txtTotalPlts").data("kendoNumericTextBox").value(TotalPlts);
            $("#txtTotalCases").data("kendoNumericTextBox").value(TotalCases);

            $('#Items').data('kendoGrid').dataSource.read();
            updateLoadSummaryData();
        }

        function totalPackageCtChanged(e) {
            
            if  (typeof ($("#txtTotalWgt").data("kendoNumericTextBox")) !== 'undefined' 
                && typeof ($("#txtTotalCases").data("kendoNumericTextBox")) !== 'undefined'
                && typeof ($("#txtTotalPlts").data("kendoNumericTextBox")) !== 'undefined')  {
                var weight =isNull(parseInt($("#txtTotalWgt").data("kendoNumericTextBox").value()),dWeight);           
                var qty = isNull(parseInt($("#txtTotalCases").data("kendoNumericTextBox").value()),1); 
                var plt = isNull(parseInt($("#txtTotalPlts").data("kendoNumericTextBox").value()),1);
                var fakClass = $("#txtClass").data("kendoMaskedTextBox").value();
                updatePackagesOnChange(weight, qty, plt, fakClass);
            }
        }

        function fakClassChanged(e) {

            if (typeof ($("#txtTotalWgt").data("kendoNumericTextBox")) !== 'undefined'
                && typeof ($("#txtTotalCases").data("kendoNumericTextBox")) !== 'undefined'
                && typeof ($("#txtTotalPlts").data("kendoNumericTextBox")) !== 'undefined') {
                var weight = isNull(parseInt($("#txtTotalWgt").data("kendoNumericTextBox").value()), dWeight);
                var qty = isNull(parseInt($("#txtTotalCases").data("kendoNumericTextBox").value()), 1);
                var plt = isNull(parseInt($("#txtTotalPlts").data("kendoNumericTextBox").value()), 1);
                var fakClass = $("#txtClass").data("kendoMaskedTextBox").value();
                updatePackagesOnChange(weight, qty, plt, fakClass);
            }
        }

        //********************** End Modify by RHR for v-8.4.0.002 on  05/07/2021  *************************
       
        //********************** Start PkgGrid Widget Code **********************
        var wndBookPkgGridEdit = kendo.ui.Window;
        var wdgtBookPkgGridEdit = new NGLEditWindCtrl();

        function openAddNewBookPkgGridWindow(e, fk) {
            if ((typeof (execBeforeBookPkgGridInsert) === 'undefined' || ngl.isFunction(execBeforeBookPkgGridInsert) === false) || (execBeforeBookPkgGridInsert(e, fk, wdgtBookPkgGridEdit) === true)) {
                wdgtBookPkgGridEdit.data = null;
                wdgtBookPkgGridEdit.show();
            }
        }
        
        function openEditBookPkgGridWindow(e) {
            var data = this.dataItem($(e.currentTarget).closest("tr"));                
            wdgtBookPkgGridEdit.data = data;
            wdgtBookPkgGridEdit.show();
            //wdgtBookPkgGridEdit.read(data.BookPkgControl);
        }

        function deleteBookPkgGridRecord(iRet, data) {
            if (typeof (data) === 'undefined' || data === null ) { return; }
            orderitems.splice(getIndexByItemNumber(data.ItemNumber), 1);
            $('#Items').data('kendoGrid').dataSource.read();
        }

        function confirmDeleteBookPkgGridRecord(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) === 'undefined' || ngl.isObject(item) == false) { return; }
            ngl.OkCancelConfirmation("Delete Selected Packages Record", "Warning! This action cannot be undone. Are you sure?", 400, 400, null, "deleteBookPkgGridRecord", item);
        }

        function NGLBookPkgGridClass() {
            ItemNumber: null;
            Description: null;
            FreightClass: null;
            Weight: null;
            Quantity: null;
            PalletCount: null;
            PackageType: null;
            Length: null;
            Width: null;
            Height: null;
            Stackable: null;
            BookPkgPkgDescControl: null;
        }
            
        var objBookPkgGridDataFields = [
            { fieldID: "33389", fieldTagID: "id62120181107153246430266", fieldCaption: "Item Number", fieldName: "ItemNumber", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: true, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 50, fieldVisible: false, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 101, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33580", fieldTagID: "id6229201905011315073981309", fieldCaption: "Pkg Desc", fieldName: "BookPkgPkgDescControl", fieldDefaultValue: "", fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetUserDynamicList", fieldAPIFilterID: "37", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 1, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33382", fieldTagID: "id62120181107153246443180", fieldCaption: "Package Type", fieldName: "PackageType", fieldDefaultValue: sPkgType, fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: false, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetStaticListDESC", fieldAPIFilterID: "64", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 2, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33370", fieldTagID: "id62120181107153246431292", fieldCaption: "Count", fieldName: "PalletCount", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 3, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33371", fieldTagID: "id62120181107153246432175", fieldCaption: "Desc", fieldName: "Description", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 255, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 4, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33372", fieldTagID: "id62120181107153246433175", fieldCaption: "FAK", fieldName: "FreightClass", fieldDefaultValue: sFreightClass, fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 5, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33373", fieldTagID: "id62120181107153246441176", fieldCaption: "NMFC", fieldName: "NMFCItem", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 6, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33374", fieldTagID: "id62120181107153246442177", fieldCaption: "Sub Class", fieldName: "NMFCSub", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 7, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33375", fieldTagID: "id62120181107153246437178", fieldCaption: "Length", fieldName: "Length", fieldDefaultValue: iPltLength.toString(), fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 8, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33376", fieldTagID: "id62120181107153246447178", fieldCaption: "Width", fieldName: "Width", fieldDefaultValue: iPltWidth.toString(), fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 9, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33377", fieldTagID: "id62120181107153246436176", fieldCaption: "Height", fieldName: "Height", fieldDefaultValue: iPltHeight.toString(), fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 10, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33378", fieldTagID: "id62120181107153246446180", fieldCaption: "Wgt", fieldName: "Weight", fieldDefaultValue: dWeight.toString(), fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n2}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 11, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33379", fieldTagID: "id62120181107153246444182", fieldCaption: "Stack", fieldName: "Stackable", fieldDefaultValue: "", fieldGroupSubType: 16, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "<input type='checkbox' #= Stackable ? 'checked=checked' : '' # disabled='disabled' ></input>", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 12, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "33388", fieldTagID: "id62120181107153246429175", fieldCaption: "Qty", fieldName: "Quantity", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 101, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
        ];

        // ************** Start BookPkgGrid Functions ******************
        //Widget object is wdgtBookPkgGridEdit
        var iBookPkgPkgDescControlID = '0';
        var ddlBookPkgPkgDescControl = undefined;
        //widget call back
        function BookPkgGridCB(oResults) {
            var sfakClass = sFreightClass;
            if (!oResults) { return; }
            if (oResults.source == "showWidgetCallback") {
                //if (oResults.source == "showWidgetCallback"  && iCarrTarFeesAccessorialCodeID == '0'  ){
                iBookPkgPkgDescControlID = wdgtBookPkgGridEdit.GetFieldID("BookPkgPkgDescControl");
                ddlBookPkgPkgDescControl = $("#" + iBookPkgPkgDescControlID).data("kendoDropDownList");
                if (ddlBookPkgPkgDescControl) { ddlBookPkgPkgDescControl.bind("change", BookPkgPkgDescControl_change); }
            } else if (oResults.source == "saveDataCB") {
                //update the item information and reload the grid
                var i = oResults.data
                var intItemNumIndex =  1;
                if (oResults.CRUD == "create") {                       
                    if (typeof (orderitems) === 'undefined' || orderitems === null) {
                        orderitems = new Array();
                        intItemNumIndex = 1;
                    } else {
                        for (index = 0; index < orderitems.length; ++index) {
                            var item = orderitems[index];
                            var iItemNbr = parseInt(item.ItemNumber);
                            if (isNaN(iItemNbr) == false && iItemNbr > intItemNumIndex) { intItemNumIndex =  iItemNbr + 1; }                                
                        }
                    }
                    var item = new rateReqItem();
                    if (intItemNumIndex < 1 ) {intItemNumIndex = 1;}
                    item.ItemNumber = intItemNumIndex + 1; // add one to the highest item number to create a unique index
                    item.Description = i.Description;
                    item.FreightClass = i.FreightClass;
                    item.Weight = i.Weight;
                    item.PackageType = i.PackageType;
                    item.PalletCount = i.PalletCount;
                    item.Quantity = i.Quantity;
                    item.Stackable = i.Stackable;
                    item.Length = i.Length;
                    item.Width = i.Width;
                    item.Height = i.Height;
                    item.NMFCItem = i.NMFCItem;
                    item.NMFCSub = i.NMFCSub;
                    orderitems.push(item)
                } else {
                    if (orderitems.length > 0) {                      
                        for (index = 0; index < orderitems.length; ++index) {
                            var item = orderitems[index];
                            if (item.ItemNumber == i.ItemNumber) {
                                item.Description = i.Description;
                                item.FreightClass = i.FreightClass;
                                item.Weight = i.Weight;
                                item.PackageType = i.PackageType;
                                item.PalletCount = i.PalletCount;
                                item.Quantity = i.Quantity;
                                item.Stackable = i.Stackable;
                                item.Length = i.Length;
                                item.Width = i.Width;
                                item.Height = i.Height;
                                item.NMFCItem = i.NMFCItem;
                                item.NMFCSub = i.NMFCSub;
                                break;
                            }
                        }
                    }
                }
                if (orderitems.length > 0) {
                    for (index = 0; index < orderitems.length; ++index) {
                        var item = orderitems[index];
                        TotalCases += parseInt(item.Quantity);
                        TotalWgt += parseFloat(item.Weight);
                        TotalPlts += parseFloat(item.PalletCount);
                    }
                }
                else {
                    TotalCases = 1;
                    TotalWgt = 1;
                    TotalPlts = 1;
                }
                $("#txtTotalWgt").data("kendoNumericTextBox").value(TotalWgt);
                $("#txtTotalPlts").data("kendoNumericTextBox").value(TotalPlts);
                $("#txtTotalCases").data("kendoNumericTextBox").value(TotalCases);
                $("#txtClass").data("kendoMaskedTextBox").value(sfakClass);

                $('#Items').data('kendoGrid').dataSource.read();
                //wdgtBookPkgGridEdit.close();
                //var grid = $("#Items").data("kendoNGLGrid");
                //if (grid) { grid.dataSource.read(); }                       
            }
        }
        //change event handler
        function BookPkgPkgDescControl_change(e) {
            if (ddlBookPkgPkgDescControl) {
                var iCodeValue = ddlBookPkgPkgDescControl.value();
                if (iCodeValue) { updateSelectedBookPkgPkgDesc(iCodeValue); }
            }
        }
        //read data to update children when list selection changes        
        function updateSelectedBookPkgPkgDesc(key) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("PackageDescription", key, tPage, "readBookPkgPkgDescCallback", "readBookPkgPkgDescAjaxErrorCallback", tPage);
            return true;
        }
        //read selected data call back from list
        // used to update children when list selection changes       
        function readBookPkgPkgDescCallback(data) {
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                        //on error do nothing for now
                        //oResults.error = new Error();
                        //oResults.error.name = "Read " + this.DataSourceName + " Failure";
                        //oResults.error.message = data.Errors;
                        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else if (data.Data != null) {
                        var record = data.Data[0];
                        if (typeof (record) !== 'undefined' && record != null && ngl.isObject(record)) {
                            //AccessorialVariableCode maps to CalcFormula List	CarrTarFeesVariableCode  DropDownList
                            //get the id
                            var sVal = '';
                            var sFieldID = ''
                            if ("PkgDescDesc" in record) {
                                sVal = record["PkgDescDesc"];
                                sFieldID = wdgtBookPkgGridEdit.GetFieldID("Description");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                            if ("PkgDescFAKClass" in record) {
                                sVal = record["PkgDescFAKClass"];
                                sFieldID = wdgtBookPkgGridEdit.GetFieldID("FreightClass");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                            if ("PkgDescNMFCClass" in record) {
                                sVal = record["PkgDescNMFCClass"];
                                sFieldID = wdgtBookPkgGridEdit.GetFieldID("NMFCItem");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                            if ("PkgDescNMFCSubClass" in record) {
                                sVal = record["PkgDescNMFCSubClass"];
                                sFieldID = wdgtBookPkgGridEdit.GetFieldID("NMFCSub");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                        }
                    }
                }
            } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
        }
        //handle any ajax errors
        function readBookPkgPkgDescAjaxErrorCallback(xhr, textStatus, error) {
            ngl.showErrMsg("Read Package Desciption Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tObj);
        }
        // ************** End BookPkgGrid Functions ******************

        //**********************  End Test NGL Widget Code

        //********************** Start AccessorialsGrid Widget Code **********************
        var wndAccessorialsGridEdit = kendo.ui.Window;
        var wdgtAccessorialsGridEdit = new NGLEditWindCtrl();
            
        function openAddNewAccessorialsGridWindow(e, fk) {
            if ((typeof (execBeforeBAccessorialsGridInsert) === 'undefined' || ngl.isFunction(execBeforeBookPkgGridInsert) === false) || (execBeforeBookPkgGridInsert(e, fk, wdgtBookPkgGridEdit) === true)) {
                wdgtAccessorialsGridEdit.data = null;
                wdgtAccessorialsGridEdit.show();
            }
        }
            
        function openEditAccessorialsGridWindow(e) {
            var data = this.dataItem($(e.currentTarget).closest("tr"));                
            wdgtAccessorialsGridEdit.data = data;
            wdgtAccessorialsGridEdit.show();
        }

        function deleteAccessorialsGridRecord(iRet, data) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }
            orderAccessorials.splice(getIndexByFeeNumber(data.FeeNumber), 1);
            $('#Accessorials').data('kendoGrid').dataSource.read();
        }

        function confirmDeleteAccessorialsGridRecord(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) === 'undefined' || ngl.isObject(item) == false) { return; }
            ngl.OkCancelConfirmation("Delete Selected Accessorial Record", "Warning! This action cannot be undone.  Are you sure?", 400, 400, null, "deleteAccessorialsGridRecord", item);
        }

        function NGLAccessorialsGridClass() {
            FeeNumber: null;
            Control: null;
            Code: null;
            Name: null;
            Desc: null;
            Value: null;
        }
        // Modified by RHR for v-8.2.1.004 on 11/26/2019
        //  changed fieldCaption for Control to "Select Fee"
        var objAccessorialsGridDataFields = [
            { fieldID: "10000001", fieldTagID: "id10000001", fieldCaption: "Fee Number", fieldName: "FeeNumber", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: true, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 50, fieldVisible: false, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 101, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }       
            , { fieldID: "10000002", fieldTagID: "id10000002", fieldCaption: "Select Fee", fieldName: "Control", fieldDefaultValue: "", fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetUserDynamicList", fieldAPIFilterID: "36", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 1, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "10000003", fieldTagID: "id10000003", fieldCaption: "Code", fieldName: "Code", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 2, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "10000004", fieldTagID: "id10000004", fieldCaption: "Name", fieldName: "Name", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 3, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "10000005", fieldTagID: "id10000005", fieldCaption: "Description", fieldName: "Desc", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 255, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 4, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
            , { fieldID: "10000006", fieldTagID: "id10000006", fieldCaption: "Value", fieldName: "Value", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:c2}", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id509201911071350538611498", fieldSequenceNo: 5, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
        ];

        // ************** Start AccessorialsGrid Functions ******************

        //Widget object is wdgtBookPkgGridEdit
        var iAccessorialsCodeControlID = '0';
        var ddlAccessorialsCodeControl = undefined;
        //widget call back
        function AccessorialsGridCB(oResults) {
            if (!oResults) { return; }
            if (oResults.source == "showWidgetCallback") {
                //if (oResults.source == "showWidgetCallback"  && iCarrTarFeesAccessorialCodeID == '0'  ){
                iAccessorialsCodeControlID = wdgtAccessorialsGridEdit.GetFieldID("Control");
                ddlAccessorialsCodeControl = $("#" + iAccessorialsCodeControlID).data("kendoDropDownList");
                if (ddlAccessorialsCodeControl) { ddlAccessorialsCodeControl.bind("change", AccessorialsGridControl_change); }
            } else if (oResults.source == "saveDataCB") {
                //update the accessorial information and reload the grid
                var i = oResults.data
                var intItemNumIndex =  1;
                if (oResults.CRUD == "create") {                 
                    if (typeof (orderAccessorials) === 'undefined' || orderAccessorials === null) {
                        orderAccessorials = new Array();
                        intItemNumIndex = 1;
                    } else {
                        for (index = 0; index < orderAccessorials.length; ++index) {
                            var item = orderAccessorials[index];
                            var iFeeNbr = parseInt(item.FeeNumber);
                            if (isNaN(iFeeNbr) == false && iFeeNbr  > intItemNumIndex) { intItemNumIndex = iFeeNbr + 1; }                                
                        }
                    }
                    var item = new NGLAPIAccessorial();
                    if (intItemNumIndex < 1 ) {intItemNumIndex = 1;}
                    item.FeeNumber = intItemNumIndex + 1; //add one to the highest fee number  to create a unique index
                    item.Control = i.Control;
                    item.Code = i.Code;
                    item.Name = i.Name;
                    item.Desc = i.Desc;
                    item.Value = i.Value;
                    orderAccessorials.push(item)
                } else {
                    if (orderAccessorials.length > 0) {                      
                        for (index = 0; index < orderAccessorials.length; ++index) {
                            var item = orderAccessorials[index];
                            if (item.FeeNumber == i.FeeNumber) {
                                item.Control = i.Control;
                                item.Code = i.Code;
                                item.Name = i.Name;
                                item.Desc = i.Desc;
                                item.Value = i.Value;
                                break;
                            }
                        }
                    }
                }                
                $('#Accessorials').data('kendoGrid').dataSource.read();                        
            }
        }
        //change event handler
        function AccessorialsGridControl_change(e) {
            if (ddlAccessorialsCodeControl) {
                var iCodeValue = ddlAccessorialsCodeControl.value();
                if (iCodeValue) { updateSelectedAccessorialsCode(iCodeValue); }
            }
        }
        //read data to update children when list selection changes        
        function updateSelectedAccessorialsCode(key) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("Accessorials/GetNGLAPICode", key, tPage, "readAccessorialsCodeCallback", "readAccessorialsCodeAjaxErrorCallback", tPage);
            return true;
        }
        //read selected data call back from list
        // used to update children when list selection changes       
        function readAccessorialsCodeCallback(data) {
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                        //on error do nothing for now
                        //oResults.error = new Error();
                        //oResults.error.name = "Read " + this.DataSourceName + " Failure";
                        //oResults.error.message = data.Errors;
                        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else if (data.Data != null) {
                        var record = data.Data[0];
                        if (typeof (record) !== 'undefined' && record != null && ngl.isObject(record)) {
                            //AccessorialVariableCode maps to CalcFormula List	CarrTarFeesVariableCode  DropDownList
                            //get the id
                            var sVal = '';
                            var sFieldID = ''
                            if ("NACCode" in record) {
                                sVal = record["NACCode"];
                                sFieldID = wdgtAccessorialsGridEdit.GetFieldID("Code");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                            if ("NACDesc" in record) {
                                sVal = record["NACDesc"];
                                sFieldID = wdgtAccessorialsGridEdit.GetFieldID("Desc");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }
                            if ("NACName" in record) {
                                sVal = record["NACName"];
                                sFieldID = wdgtAccessorialsGridEdit.GetFieldID("Name");
                                var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                if (domItem) { domItem.value(sVal); }
                            }                           
                        }
                    }
                }
            } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
        }
        //handle any ajax errors
        function readAccessorialsCodeAjaxErrorCallback(xhr, textStatus, error) {
            ngl.showErrMsg("Read Accessorials Codes Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tObj);
        }

        // ************** End AccessorialsGrid Functions ******************

        //**********************  End AccessorialsGrid Widget Code
       
        // start widgit configuration
        var winDispatchingDialog = kendo.ui.Window;
        var winDispatchReport = kendo.ui.Window;
        var winBOLReport  = kendo.ui.Window;
        var oDispatchingDialogCtrl = new DispatchingDialogCtrl();     
        var oDispatchReportCtrl = new DispatchingReportCtrl();
        var oBOLReportCtrl = new BOLReportCtrl();
        var oDispatchData = new Dispatch();
        //create widgit call backs
        function oDispatchingDialogSelectCB(results){  
            //debugger;
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
                data = oDispatchingDialogCtrl.data;
                if (blnDispatchedFromPageQuote == true) {
                    $("#txtorigCompName").data("kendoMaskedTextBox").value(data.Origin.Name);
                    $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(data.Origin.Address1);
                    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);                      
                    $("#txtdestCompName").data("kendoMaskedTextBox").value(data.Destination.Name);
                    $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(data.Destination.Address1);
                    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                } 
            } catch (err) {
                //do nothing
            }
        }
        function oDispatchingDialogSaveCB(results){
            //debugger;
            var data = new Dispatch();
            if (typeof (results)  === 'undefined' || results == null  ) { return;}
            if (typeof (results.data) === 'undefined' ||  results.data == null || ngl.isArray(results.data) == false) { return;}
            try{
                data = results.data[0];            
                if (blnDispatchedFromPageQuote == true) {
                    $("#txtorigCompAddress1").val(data.Origin.Address1);
                    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    $("#txtdestCompAddress1").val(data.Destination.Address1);
                    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                }
                oDispatchReportCtrl.data = data;
                oDispatchReportCtrl.show();
            } catch (err) {
               //do nothing
            }
        }
        function oDispatchingDialogCloseCB(results){ 
            //debugger;
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
                data = oDispatchingDialogCtrl.data;       
                if (blnDispatchedFromPageQuote == true) {
                    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                }
            } catch (err) {
                //do nothing
            }
        }
        function oDispatchingDialogReadCB(results){ 
            //debugger;
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
                data = oDispatchingDialogCtrl.data;           
                if (blnDispatchedFromPageQuote == true) {
                    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail); 
                }
            } catch (err) {
                //do nothing
            }
        }

        function oBOLReportSelectCB(results){ return; }
        function oBOLReportSaveCB(results){ return; }
        function oBOLReportCloseCB(results){ return; }
        function oBOLReportReadCB(results){ return; }

        function oDispatchingReportSelectCB(results){ return; }
        function oDispatchingReportSaveCB(results){ return; }
        function oDispatchingReportCloseCB(results){  
            //Modified by RHR for v-8.2 on 02/20/2019
            ngl.readDataSource(oDispatchGrid);
            ngl.readDataSource(oHistQuotesGrid);          
            return;
        }
        function oDispatchingReportReadCB(results){ return; }
        
        //End widgit configuration

        //Start page properties
        var orderitems = new Array()
        var orderstops = new Array()
        var orderAccessorials = new Array();
        var intStopIndex = 0;
        var intItemIndex = 0;          
        var intItemStopIndex= 0;             
        var intItemNumIndex = 0; //Doesn't get reset until after the order is complete
        var intSelectedStopIndex = 0;
        var intTotalStops = 0;
        var intFeeIndex = 0;

        var P44RateQuotedataSource = kendo.data.DataSource;
        var dsAddressBook = kendo.data.DataSource;

        var TotalCases = 0;
        var TotalWgt = 0;
        var TotalPlts = 0;
        var alertMSg = "";
        var zipWin = kendo.ui.Window;
        var winItems = kendo.ui.Window;
        var winAccessorials = kendo.ui.Window;
        var AccessorialsGrid = kendo.ui.Grid;
        var winAddressBook = kendo.ui.Window;
        var blnWinItemsClosing = false;
        //Start Content Management Call back 
        function printwinItems(){
            //debugger;
            var kinWin = $("#winItems").kendoWindow().data("kendoWindow");
            var winWrapper = kinWin.wrapper;
            winWrapper.removeClass("k-window");
            winWrapper.addClass("printable");
            //put pring code here
            window.print();
            winWrapper.removeClass("printable");
            winWrapper.addClass("k-window");      
        }

        var oHistoricalQuoteBid = null; 
        function dispatchHistoricalQuote(e){                      
            //alert('do dispatch')
            oHistoricalQuoteBid = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oHistoricalQuoteBid) !== 'undefined' && ngl.isObject(oHistoricalQuoteBid)) {
                oDispatchingDialogCtrl.read(oHistoricalQuoteBid.BidControl)
                //ngl.OkCancelConfirmation(
                //       "Replace Load Information With Selected",
                //       "This action will replace the current load information with the select address and item details",
                //       400,
                //       400,
                //       null,
                //       "ConfirmCopyHistoricalQuote");
            };      
        }

        var oQuoteSelectedBid = null; 
        function dispatchSelectedQuote(e){
            //debugger;
            //alert('do dispatch')
            oQuoteSelectedBid = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oQuoteSelectedBid) !== 'undefined' && ngl.isObject(oQuoteSelectedBid)) {
                oDispatchingDialogCtrl.read(oQuoteSelectedBid.BidControl)
                //ngl.OkCancelConfirmation(
                //       "Replace Load Information With Selected",
                //       "This action will replace the current load information with the select address and item details",
                //       400,
                //       400,
                //       null,
                //       "ConfirmCopyHistoricalQuote");
            };       
        }

        function PrintSelectedBOL(e){
            var oSelectedDispatch = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oSelectedDispatch) !== 'undefined' && ngl.isObject(oSelectedDispatch)) {
                oBOLReportCtrl.read(oSelectedDispatch.LoadTenderControl)
            };          
        }

        var oHistQuotesGrid = null;
        function editHistoricalQuote(e){ alert("Edit"); }

        function ConfirmCopyHistoricalQuote(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}          
            //oKendoGrid = this;
            if (typeof (oHistoricalQuoteitem) !== 'undefined' && ngl.isObject(oHistoricalQuoteitem)) {
                $("#txtorigCompName").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookOrigName);
                $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookOrigAddress1);                          
                $("#txtorigCompCountry").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookOrigCountry); 
                $("#txtorigCompCity").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookOrigCity);
                $("#txtorigCompState").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookOrigState);
                $("#txtorigCompPostalCode").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookOrigZip);
                $("#txtOrigPhone").val(oHistoricalQuoteitem.LTBookOrigPhone); 
                $("#txtOrigContactName").val(oHistoricalQuoteitem.LTBookOrigContactName); 
                $("#txtOrigContactEmail").val(oHistoricalQuoteitem.LTBookOrigContactEmail); 
                $("#txtOrigEmergencyContactPhone").val(oHistoricalQuoteitem.LTBookOrigEmergencyContactPhone); 
                $("#txtOrigEmergencyContactName").val(oHistoricalQuoteitem.LTBookOrigEmergencyContactName);                
                $("#txtdestCompName").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookDestName);
                $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookDestAddress1);
                $("#txtdestCompCountry").data("kendoMaskedTextBox").value(oHistoricalQuoteitem.LTBookDestCountry);
                $("#txtdestCompCity").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookDestCity);         
                $("#txtdestCompState").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookDestState);
                $("#txtdestCompPostalCode").data("kendoComboBox").value(oHistoricalQuoteitem.LTBookDestZip);
                $("#txtDestPhone").val(oHistoricalQuoteitem.LTBookDestPhone); 
                $("#txtDestContactName").val(oHistoricalQuoteitem.LTBookDestContactName); 
                $("#txtDestContactEmail").val(oHistoricalQuoteitem.LTBookDestContactEmail); 
                $("#txtDestEmergencyContactPhone").val(oHistoricalQuoteitem.LTBookDestEmergencyContactPhone); 
                $("#txtDestEmergencyContactName").val(oHistoricalQuoteitem.LTBookDestEmergencyContactName);     
                $("#txtSHID").data("kendoMaskedTextBox").value('(Auto)');
                $("#txtTotalCases").data("kendoNumericTextBox").value(oHistoricalQuoteitem.LTBookTotalCases); 
                $("#txtTotalWgt").data("kendoNumericTextBox").value(oHistoricalQuoteitem.LTBookTotalWgt); 
                $("#txtTotalPlts").data("kendoNumericTextBox").value(oHistoricalQuoteitem.LTBookTotalPL);
                $("#txtClass").data("kendoMaskedTextBox").value(sFreightClass);
                //s.TotalCube = 0;
                $("#txtShipDate").data("kendoDatePicker").value(oHistoricalQuoteitem.LTBookDateLoad);
                $("#txtDeliveryDate").data("kendoDatePicker").value(oHistoricalQuoteitem.LTBookDateRequired);
                //debugger;
                if (ngl.isNullOrWhitespace(oHistoricalQuoteitem.LTLLaneWeightUnit)) { $("#txtWeightUnit").data("kendoComboBox").value(sWeightUnit) } else { $("#txtWeightUnit").data("kendoComboBox").value(oHistoricalQuoteitem.LTLLaneWeightUnit); }
                if (ngl.isNullOrWhitespace(oHistoricalQuoteitem.LTLLaneLengthUnit)) { $("#txtLengthUnit").data("kendoComboBox").value(sLengthUnit); } else { $("#txtLengthUnit").data("kendoComboBox").value(oHistoricalQuoteitem.LTLLaneLengthUnit); }
                // modified by RHR for v-8.5.4.001 on 06/29/2023
                //alert("need to write a function to read and convert CommCode to TariffTempType");
                //var dTempData = $("#ddlLoadTemp").data("kendoDropDownList");
                //dropdownlist.value(oHistoricalQuoteitem.LTTariffTempType);
                //dropdownlist.trigger("change");
                orderitems = new Array();
                //Modified by RHR for v-8.2 on 02/20/2019
                ngl.readDataSource($('#Items').data('kendoGrid'));
                updateLoadSummaryData();
                var LoadTenderControl = oHistoricalQuoteitem.LoadTenderControl;
                GetRateRequestItems(LoadTenderControl);
                GetLoadTenderFees(LoadTenderControl);
                //var ardata = new AcceptorReject();
                //ardata.BookControl = oHistoricalQuoteitem.BookControl;
                //ardata.AcceptRejectCode = 0;  //accepted
                //ardata.BookTrackComment = '';
                //var restServiceWidget = new nglRESTCRUDCtrl()
                //restServiceWidget.update("Tendered/PostSave", ardata, oPageObject, "loadAcceptedCallBack", "saveAjaxErrorCallback")
            }
        }
       
        var oHistoricalQuoteitem = null; 
        function copyHistoricalQuote(e){
            oHistoricalQuoteitem = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oHistoricalQuoteitem) !== 'undefined' && ngl.isObject(oHistoricalQuoteitem)) {
                ngl.OkCancelConfirmation(
                       "Replace Load Information With Selected",
                       "This action will replace the current load information with the select address and item details",
                       400,
                       400,
                       null,
                       "ConfirmCopyHistoricalQuote");
            };      
        }

        function ConfirmCopyDispatchedLoad(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}       
            //oKendoGrid = this;
            if (typeof (oDispatchedLoaditem) !== 'undefined' && ngl.isObject(oDispatchedLoaditem)) {
                $("#txtorigCompName").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookOrigName);
                $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookOrigAddress1);                          
                $("#txtorigCompCountry").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookOrigCountry); 
                $("#txtorigCompCity").data("kendoComboBox").value(oDispatchedLoaditem.LTBookOrigCity);
                $("#txtorigCompState").data("kendoComboBox").value(oDispatchedLoaditem.LTBookOrigState);
                $("#txtorigCompPostalCode").data("kendoComboBox").value(oDispatchedLoaditem.LTBookOrigZip);
                $("#txtOrigPhone").val(oDispatchedLoaditem.LTBookOrigPhone); 
                $("#txtOrigContactName").val(oDispatchedLoaditem.LTBookOrigContactName); 
                $("#txtOrigContactEmail").val(oDispatchedLoaditem.LTBookOrigContactEmail); 
                $("#txtOrigEmergencyContactPhone").val(oDispatchedLoaditem.LTBookOrigEmergencyContactPhone); 
                $("#txtOrigEmergencyContactName").val(oDispatchedLoaditem.LTBookOrigEmergencyContactName); 
                $("#txtdestCompName").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookDestName);
                $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookDestAddress1);
                $("#txtdestCompCountry").data("kendoMaskedTextBox").value(oDispatchedLoaditem.LTBookDestCountry);
                $("#txtdestCompCity").data("kendoComboBox").value(oDispatchedLoaditem.LTBookDestCity);         
                $("#txtdestCompState").data("kendoComboBox").value(oDispatchedLoaditem.LTBookDestState);
                $("#txtdestCompPostalCode").data("kendoComboBox").value(oDispatchedLoaditem.LTBookDestZip);
                $("#txtDestPhone").val(oDispatchedLoaditem.LTBookDestPhone); 
                $("#txtDestContactName").val(oDispatchedLoaditem.LTBookDestContactName); 
                $("#txtDestContactEmail").val(oDispatchedLoaditem.LTBookDestContactEmail); 
                $("#txtDestEmergencyContactPhone").val(oDispatchedLoaditem.LTBookDestEmergencyContactPhone); 
                $("#txtDestEmergencyContactName").val(oDispatchedLoaditem.LTBookDestEmergencyContactName);      
                $("#txtSHID").data("kendoMaskedTextBox").value('(Auto)');
                $("#txtTotalCases").data("kendoNumericTextBox").value(oDispatchedLoaditem.LTBookTotalCases); 
                $("#txtTotalWgt").data("kendoNumericTextBox").value(oDispatchedLoaditem.LTBookTotalWgt); 
                $("#txtTotalPlts").data("kendoNumericTextBox").value(oDispatchedLoaditem.LTBookTotalPL); 
                $("#txtClass").data("kendoMaskedTextBox").value(sFreightClass);
                //s.TotalCube = 0;
                $("#txtShipDate").data("kendoDatePicker").value(oDispatchedLoaditem.LTBookDateLoad);
                $("#txtDeliveryDate").data("kendoDatePicker").value(oDispatchedLoaditem.LTBookDateRequired);
                //debugger;
                if (ngl.isNullOrWhitespace(oDispatchedLoaditem.LTLLaneWeightUnit)) { $("#txtWeightUnit").data("kendoComboBox").value(sWeightUnit); } else { $("#txtWeightUnit").data("kendoComboBox").value(oDispatchedLoaditem.LTLLaneWeightUnit); }
                if (ngl.isNullOrWhitespace(oDispatchedLoaditem.oDispatchedLoaditem)) { $("#txtLengthUnit").data("kendoComboBox").value(sLengthUnit); } else { $("#txtLengthUnit").data("kendoComboBox").value(oDispatchedLoaditem.LTLLaneLengthUnit); }
               
                orderitems = new Array();
                //Modified by RHR for v-8.2 on 02/20/2019
                ngl.readDataSource($('#Items').data('kendoGrid'));                
                updateLoadSummaryData();
                var LoadTenderControl = oDispatchedLoaditem.LoadTenderControl;
                GetRateRequestItems(LoadTenderControl);
                GetLoadTenderFees(LoadTenderControl);
            }
        }
        
        var oDispatchedLoaditem = null; 
        function copyDispatchedLoad(e) {
            oDispatchedLoaditem = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oDispatchedLoaditem) !== 'undefined' && ngl.isObject(oDispatchedLoaditem)) {
                ngl.OkCancelConfirmation(
                       "Replace Load Information With Selected",
                       "This action will replace the current load information with the select address and item details",
                       400,
                       400,
                       null,
                       "ConfirmCopyDispatchedLoad");
            };
        }


        //************* Call Back Functions **********************

        function HistBidsGridDataBoundCallBack(e, tGrid, sKey) {
           // alert("HistBidsGridDataBoundCallBack");
            kendo.ui.progress($(document.body), false);
            // get the index of the Status column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "Status" + "]").index();
            //var gridWrapper = e.sender.wrapper;
            //var gridDataTable = e.sender.table;
            //var gridDataArea = gridDataTable.closest(".k-grid-content");
            //gridWrapper.toggleClass("no-scrollbar", gridDataTable[0].offsetHeight < gridDataArea[0].offsetHeight);           
            //oQuotesGrid = tGrid;
            var ds = tGrid.dataSource.data();
            var oLen = ds.length;
            //if (oLen < 1) { oQuotesGrid.show = false; } else { oQuotesGrid.show = true; }
            //alert("Grid Len: " + oLen.toString());
            for (var j = 0; j < ds.length; j++) {
                if (typeof (ds[j].BidSelectedForExport) !== 'undefined' && ds[j].BidSelectedForExport != null) {
                    if (ds[j].BidSelectedForExport == 0) {
                        var item = tGrid.dataSource.get(ds[j].BidControl); //Get by PK
                        //console.log('btn');

                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportHistQuoteBid");
                        if (typeof (btn) !== 'undefined' && btn != null) {
                            btn.removeAttr("style");
                            //btn.visible = false;
                        }
                    }
                    else {
                        var item = tGrid.dataSource.get(ds[j].BidControl); //Get by PK                    
                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportHistQuoteBid");
                        if (typeof (btn) !== 'undefined' && btn != null) {
                            btn.css("color", "red");
                        }
                    }
                }
            }
        }

        function QuotedLoadsGetStringData(s){ return ''; }

        var sHistQuotesGridKey = '';
        function QuotedLoadsDataBoundCallBack(e, tGrid, sKey) {
            sHistQuotesGridKey = sKey;
            kendo.ui.progress($(document.body), false);           
            // get the index of the Status column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "Status" + "]").index();
            //var gridWrapper = e.sender.wrapper;
            //var gridDataTable = e.sender.table;
            //var gridDataArea = gridDataTable.closest(".k-grid-content");
            //gridWrapper.toggleClass("no-scrollbar", gridDataTable[0].offsetHeight < gridDataArea[0].offsetHeight);      
            oHistQuotesGrid = tGrid;
            var ds = tGrid.dataSource.data();
            var oLen = ds.length;
            //if (oLen < 1) { oQuotesGrid.show = false; } else { oQuotesGrid.show = true; }
            //alert("Grid Len: " + oLen.toString());
            for (var j = 0; j < ds.length; j++) {
                if (typeof (ds[j].LTSelectedForExport) !== 'undefined' && ds[j].LTSelectedForExport != null) {
                    if (ds[j].LTSelectedForExport == 0) {
                        var item = tGrid.dataSource.get(ds[j].LoadTenderControl); //Get by PK
                        //console.log('btn');

                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportHistQuote");
                        if (typeof (btn) !== 'undefined' && btn != null) {
                            btn.removeAttr("style");
                            //btn.visible = false;
                        }
                    }
                    else {
                        var item = tGrid.dataSource.get(ds[j].LoadTenderControl); //Get by PK                    
                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportHistQuote");
                        if (typeof (btn) !== 'undefined' && btn != null) {
                            btn.css("color", "red");
                        }
                    }
                }
            }
        }
        var oDispatchGrid = null;
        function DispatchedGridDataBoundCallBack(e, tGrid, sKey){
            kendo.ui.progress($(document.body), false);     
            // get the index of the Status column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "Status" + "]").index();
            //var gridWrapper = e.sender.wrapper;
            //var gridDataTable = e.sender.table;
            //var gridDataArea = gridDataTable.closest(".k-grid-content");
            //gridWrapper.toggleClass("no-scrollbar", gridDataTable[0].offsetHeight < gridDataArea[0].offsetHeight);   
            oDispatchGrid = tGrid;
        }

        var oQuotesGrid = null;
        function QuotesGetStringData(s)
        {           
            //s.filterName = "BidLoadTenderControl"
            //s.filterValue = sBidLoadTenderControlVal
            s.ParentControl = sBidLoadTenderControlVal;           
            return '';
        }

        function QuotesDataBoundCallBack(e, tGrid, sKey){            
            kendo.ui.progress($(document.body), false);
            // get the index of the Status column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "Status" + "]").index();          
            //var gridWrapper = e.sender.wrapper;
            //var gridDataTable = e.sender.table;
            //var gridDataArea = gridDataTable.closest(".k-grid-content");
            //gridWrapper.toggleClass("no-scrollbar", gridDataTable[0].offsetHeight < gridDataArea[0].offsetHeight);           
            oQuotesGrid = tGrid;
            var ds = tGrid.dataSource.data();
            var oLen = ds.length;
            if (oLen < 1) { oQuotesGrid.show = false; } else { oQuotesGrid.show = true; }
            //alert("Grid Len: " + oLen.toString());
            for (var j = 0; j < ds.length; j++) {
                if (typeof (ds[j].BidSelectedForExport) !== 'undefined' && ds[j].BidSelectedForExport != null ) {
                    if (ds[j].BidSelectedForExport == 0) {
                        var item = tGrid.dataSource.get(ds[j].BidControl); //Get by PK
                        //console.log('btn');
                       
                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportQuote");
                        if (typeof (btn) !== 'undefined' && btn != null) {                           
                            btn.removeAttr("style");
                            //btn.visible = false;
                        }
                    }
                    else {
                        var item = tGrid.dataSource.get(ds[j].BidControl); //Get by PK                    
                        var btn = tGrid.tbody.find("tr[data-uid='" + item.uid + "'] .k-grid-btnExportQuote");
                        if (typeof (btn) !== 'undefined' && btn != null) {                           
                            btn.css("color", "red");
                        }
                    }
                }                
            }
        }

        function PostRateRequestCallback(data) {
            kendo.ui.progress($(document.body), false);
            var oResults = new nglEventParameters();
            oResults.source = "PostRateRequestCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            sBidLoadTenderControlVal = "0"
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    //debugger;
                    //console.log("Post Rate Request Data");
                    //console.log(data);
                    if (typeof (errWarnData) !== 'undefined') {
                        console.log(data);
                        errWarnData = data;
                    } 
                    //if (typeof (wdgtNGLErrWarnMsgLogCtrlDialog) !== 'undefined' && wdgtNGLErrWarnMsgLogCtrlDialog != null ) {
                    //    wdgtNGLErrWarnMsgLogCtrlDialog.show(data);
                    //}
                    if (data.Errors != null) {
                        oResults.error = new Error();
                        if (data.StatusCode === 203) { oResults.error.name = "Authorization Timeout"; } else { oResults.error.name = "Access Denied"; }
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                   
                    this.rData = data.Data;
                    if (data.Data != null) {                          
                        oResults.data = data.Data;
                        sBidLoadTenderControlVal = data.Data[0]
                        //this.edit(data.Data)
                        //oResults.datatype = "integer";
                        //oResults.msg = "Success." + "  Your changes have been saved."
                        //ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                   
                } else { oResults.msg = "Success no data returned"; ngl.showSuccessMsg(oResults.msg, tObj); }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }
            blnWinItemsClosing = false;
            //console.log("sBidLoadTenderControlVal");
            //console.log(sBidLoadTenderControlVal);
            if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                kendo.ui.progress($(document.body), true);
                //Modified by RHR for v-8.2 on 02/20/2019
                ngl.readDataSource(oQuotesGrid); 
                ngl.readDataSource(oHistQuotesGrid);                 
            }
        }

        function PostRateRequestAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($(document.body), false);
            var oResults = new nglEventParameters();
            var tObj = this;
            sBidLoadTenderControlVal = "0"
            oResults.source = "PostRateRequestAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Post Rate Request Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            blnWinItemsClosing = false;           
            if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                kendo.ui.progress($(document.body), true);
                //oQuotesGrid.empty();
                //Modified by RHR for v-8.2 on 02/20/2019
                ngl.readDataSource(oQuotesGrid);
            }
        }

        function GetRateRequestItemsCallback(data) {           
            var oResults = new nglEventParameters();
            oResults.source = "GetRateRequestItemsCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (data.Errors != null) {
                        oResults.error = new Error();
                        if (data.StatusCode === 203) { oResults.error.name = "Authorization Timeout"; } else { oResults.error.name = "Access Denied"; }
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {
                        this.rData = data.Data;
                        if (data.Data != null) {                      
                            oResults.data = data.Data;
                            //sBidLoadTenderControlVal = data.Data[0]
                            orderitems = data.Data;
                            //Modified by RHR for v-8.5.4.006 on 04/18/2024 update FAK Class
                            if (typeof (orderitems) !== 'undefined' && orderitems[0].FreightClass !== undefined) {
                                console.log(orderitems[0].FreightClass);
                                $("#txtClass").data("kendoMaskedTextBox").value(orderitems[0].FreightClass);
                            }
                            //Modified by RHR for v-8.2 on 02/20/2019
                            ngl.readDataSource($('#Items').data('kendoGrid')); 
                            //this.edit(data.Data)
                            //oResults.datatype = "integer";
                            //oResults.msg = "Success." + "  Your changes have been saved."
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                        else {                            
                            oResults.error = new Error();
                            oResults.error.name = "Invalid Request";
                            oResults.error.message = "No Data available.";
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                    }
                } else { oResults.msg = "Success no data returned"; ngl.showSuccessMsg(oResults.msg, tObj); }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }         
            btnManageItems_Click();
        }

        function GetRateRequestItemsAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            sBidLoadTenderControlVal = "0"
            oResults.source = "GetRateRequestItemsAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get Rate Request Items Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            btnManageItems_Click();
        }

        function GetLoadTenderFeesCallback(data) {           
            var oResults = new nglEventParameters();
            oResults.source = "GetLoadTenderFeesCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (data.Errors != null) {
                        oResults.error = new Error();
                        if (data.StatusCode === 203) { oResults.error.name = "Authorization Timeout"; } else { oResults.error.name = "Access Denied"; }
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {
                        this.rData = data.Data;
                        if (data.Data != null && ngl.isArray(data.Data)) {                            
                            oResults.data = data.Data;                       
                            orderAccessorials = data.Data;
                            for (var i = 0; i < orderAccessorials.length; i++) {
                                intFeeIndex += 1;
                                var fee = orderAccessorials[i];
                                fee.FeeNumber = intFeeIndex;                            
                            }                          
                            //Modified by RHR for v-8.2 on 02/20/2019
                            ngl.readDataSource($('#Accessorials').data('kendoGrid')); 
                            $("#txtFeeIndex").val(0);                        
                        }
                        else {                            
                            oResults.error = new Error();
                            oResults.error.name = "Invalid Request";
                            oResults.error.message = "No Data available.";
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                    }
                } else { oResults.msg = "Success no data returned"; ngl.showSuccessMsg(oResults.msg, tObj); }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }                        
        }

        function GetLoadTenderFeesAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            sBidLoadTenderControlVal = "0"
            oResults.source = "GetLoadTenderFeesAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Get Tendered Fees Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            btnManageItems_Click();
        }

        function savePostPageSettingSuccessCallback(results) {
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error) {
            //for now do nothing when we save the pk
        }

        function readGetPageSettingSuccessCallback(data) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "readGetPageSettingSuccessCallback";
            oResults.msg = 'Failed'; //set default to Failed         
            oResults.CRUD = "read";
            oResults.widget = tObj;
            var dsUserPageSettings = null;                          
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";                        
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null);                           
                    }                          
                    else {                               
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                dsUserPageSettings = data.Data[0]; 
                                oResults.msg = "Success";
                                if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                                    var psData = JSON.parse(dsUserPageSettings.value);                                                                              
                                    var blnLoadDefaults = true;               
                                    if(!ngl.isNullOrWhitespace(psData.OrigPostalCode)){
                                        blnLoadDefaults = false;               
                                        $("#txtorigCompName").data("kendoMaskedTextBox").value(psData.OrigName);
                                        $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(psData.OrigAddress1);                          
                                        $("#txtorigCompCountry").data("kendoMaskedTextBox").value(psData.OrigCountry); 
                                        $("#txtorigCompCity").data("kendoComboBox").value(psData.OrigCity);
                                        $("#txtorigCompState").data("kendoComboBox").value(psData.OrigState);
                                        $("#txtorigCompPostalCode").data("kendoComboBox").value(psData.OrigPostalCode);
                                        SetOrigContact(psData.OrigContact);
                                        $("#txtdestCompName").data("kendoMaskedTextBox").value(psData.DestName);
                                        $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(psData.DestAddress1);
                                        $("#txtdestCompCountry").data("kendoMaskedTextBox").value(psData.DestCountry);
                                        $("#txtdestCompCity").data("kendoComboBox").value(psData.DestCity);         
                                        $("#txtdestCompState").data("kendoComboBox").value(psData.DestState);
                                        $("#txtdestCompPostalCode").data("kendoComboBox").value(psData.DestPostalCode);
                                        SetDestContact(psData.DestContact);
                                        $("#txtSHID").data("kendoMaskedTextBox").value(psData.SHID);
                                        $("#txtTotalCases").data("kendoNumericTextBox").value(psData.TotalCases); 
                                        $("#txtTotalWgt").data("kendoNumericTextBox").value(psData.TotalWgt); 
                                        $("#txtTotalPlts").data("kendoNumericTextBox").value(psData.TotalPL); 
                                        $("#txtClass").data("kendoMaskedTextBox").value(psData.fakClass);
                                        //psData.TotalCube = 0;
                                        $("#txtShipDate").data("kendoDatePicker").value(psData.LoadDate);
                                        $("#txtDeliveryDate").data("kendoDatePicker").value(psData.RequiredDate);
                                        $("#txtWeightUnit").data("kendoComboBox").value(psData.WeightUnit);
                                        $("#txtLengthUnit").data("kendoComboBox").value(psData.LengthUnit); 
                                        $("#ddlLoadTemp").data("kendoDropDownList").value(psData.TariffTempType); //Modified by RHR for v-8.5.4.001 add temperature
                                        if(psData.Items != null && psData.Items.length > 0){
                                            orderitems = psData.Items;
                                            intItemNumIndex = orderitems.length;
                                            //Modified by RHR for v-8.2 on 02/20/2019
                                            ngl.readDataSource($('#Items').data('kendoGrid'));                                         
                                        }
                                        if(psData.Accessorials != null && psData.Accessorials.length > 0){
                                            orderAccessorials = psData.Accessorials;
                                            intFeeIndex = orderAccessorials.length;                                            
                                            //Modified by RHR for v-8.2 on 02/20/2019
                                            ngl.readDataSource($('#Accessorials').data('kendoGrid'));                                        
                                        }
                                        SetEmergencyContact(psData.EmergencyContact)                                       
                                    }                                                                                                         
                                    if (blnLoadDefaults == true){
                                        $("#txtorigCompName").data("kendoMaskedTextBox").value("Ship From");
                                        $("#txtdestCompName").data("kendoMaskedTextBox").value("Ship To");
                                        $("#txtSHID").data("kendoMaskedTextBox").value("(Auto)");
                                        $("#txtTotalCases").data("kendoNumericTextBox").value(1); 
                                        $("#txtTotalWgt").data("kendoNumericTextBox").value(dWeight); 
                                        $("#txtTotalPlts").data("kendoNumericTextBox").value(1); 
                                        $("#txtClass").data("kendoMaskedTextBox").value(sFreightClass);
                                        orderitems = new Array();
                                        var item1 = new rateReqItem();               
                                        item1.ItemIndex = 1;
                                        item1.LoadID = 0; 
                                        item1.ItemStopIndex = 1;
                                        item1.ItemNumber = 1;  
                                        item1.Weight = dWeight;
                                        item1.FreightClass = sFreightClass;
                                        item1.PalletCount = 1; 
                                        item1.NumPieces = 1; 
                                        item1.Description = "Item 1"; 
                                        item1.Quantity = "1"; 
                                        item1.HazmatId = "";                                     
                                        item1.Code = ""; 
                                        item1.HazmatClass = ""; 
                                        item1.IsHazmat = false; 
                                        item1.Pieces = "";
                                        item1.PackageType = sPkgType;                 
                                        item1.Length = iPltLength; 
                                        item1.Width = iPltWidth; 
                                        item1.Height = iPltHeight; 
                                        item1.Density = ""; 
                                        item1.NMFCItem = ""; 
                                        item1.NMFCSub = "";
                                        item1.Stackable = true;
                                        orderitems.push(item1)
                                        intItemNumIndex = orderitems.length;
                                        //Modified by RHR for v-8.2 on 02/20/2019
                                        ngl.readDataSource($('#Items').data('kendoGrid'));
                                    }
                                    updateLoadSummaryData();                                   
                                }                                   
                            }                              
                        }                            
                    }                        
                }                      
                if (blnSuccess === false && blnErrorShown === false) {                          
                    if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                    ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);                       
                }                   
            } catch (err) {
                ngl.showErrMsg(err.name, err.description, null);                   
            }             
        }

        function readGetPageSettingAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "readGetPageSettingAjaxErrorCallback";
            oResults.msg = 'Failed'; //set default to Failed        
            oResults.CRUD = "read"
            oResults.widget = tObj;
            oResults.error = new Error();
            oResults.error.name = "Read Page Settings Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        }


        //btnImportLoads

        //} else if (btn.id === "btnImportSpecificTariffRates") {         openImportTariffRatesWnd(PageControl);
        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnCalculateLoad"){ btnCalculateLoad_Click(); }                
            else if(btn === "openNGLErrWarnMsgLogCtrlDialog") {openNGLErrWarnMsgLogCtrlDialog();}
            else if (btn.id == "btnClear") { btnClear_Click(); }
            else if (btn.id == "btnClearActive") { ClearActiveQuotesForUser(); }
            else if(btn.id == "btnManageItems"){ btnManageItems_Click(); }
            else if (btn.id == "btnManageAccessorials") { btnManageAccessorials_Click(); }
            else if (btn.id == "btnImportLoads") { openImportRateShopLoadsWnd(PageControl) }
            else if(btn.id == "btnAddressBook"){ btnAddressBook_Click(); }
            else if(btn.id == "btnDispatch"){ Dispatch_Click(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }                
        }

        function ClearActiveQuotesForUser() {
            $.ajax({
                type: 'POST',
                url: 'api/RateShopQuote/DeleteAllQuotesForUser/0',
                contentType: "application/json; charset=utf-8",
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {                    
                    if (data.Errors != null) {
                        if (data.StatusCode === 203) { ngl.showErrMsg("Authorization Timeout", data.Errors, null); }
                        else { ngl.showErrMsg("Cannot Delete Active Quotes", data.Errors, null); }
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Active Rate Shop Quotes Failure");
                    ngl.showErrMsg("Delete Active Rate Shop Quotes failed", sMsg, null);
                }
            });
            if (typeof (oHistQuotesGrid) !== 'undefined' && ngl.isObject(oHistQuotesGrid)) {
                kendo.ui.progress($(document.body), true);
                ngl.readDataSource(oHistQuotesGrid);
            } 
        }


        //************* Page Functions **********************
        function btnAddressBook_Click(){
            var grid = $("#AddressBookGrid").data("kendoGrid");
            var autocomplete = $("#acFilter").data("kendoAutoComplete").value("");
            dsAddressBook.read();
            winAddressBook.center().open();
        }

        function btnSetOrigin_Click(){
            var grid = $("#AddressBookGrid").data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (item != null){ 
                $("#txtorigCompName").data("kendoMaskedTextBox").value(item.Name);
                $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(item.Address1);
                $("#txtorigCompAddress2").val(item.Address2);
                $("#txtorigCompAddress3").val(item.Address3);
                $("#txtorigCompCity").data("kendoComboBox").value(item.City);
                $("#txtorigCompState").data("kendoComboBox").value(item.State);
                $("#txtorigCompCountry").data("kendoMaskedTextBox").value(item.Country);
                $("#txtorigCompPostalCode").data("kendoComboBox").value(item.Zip); 
                //var dsi = grid.dataSource.data();
                //for (index = 0; index <  dsi.length; ++index){
                //    var i = dsi[index];
                //    if (i.strImg === "k-i-marker-pin-target") { i.strImg = ""; }
                //}
                //item.strImg = "k-i-marker-pin-target"
                //grid.refresh();
            }           
        }

        function btnSetDest_Click(){
            var grid = $("#AddressBookGrid").data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (item != null){ 
                $("#txtdestCompName").data("kendoMaskedTextBox").value(item.Name);
                $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(item.Address1);
                $("#txtdestCompAddress2").val(item.Address2);
                $("#txtdestCompAddress3").val(item.Address3);
                $("#txtdestCompCity").data("kendoComboBox").value(item.City);
                $("#txtdestCompState").data("kendoComboBox").value(item.State);
                $("#txtdestCompCountry").data("kendoMaskedTextBox").value(item.Country);
                $("#txtdestCompPostalCode").data("kendoComboBox").value(item.Zip);
                //var dsi = grid.dataSource.data();
                //for (index = 0; index <  dsi.length; ++index){
                //    var i = dsi[index];
                //    if (i.strImg === "k-i-marker-pin") { i.strImg = ""; }
                //}
                //item.strImg = "k-i-marker-pin"
                //grid.refresh();
            }         
        }

        var blnwinItemsEventAdded = 0;
        function btnManageItems_Click(){
            winItems = $("#winItems").kendoWindow({
                title: "Shipping Information",
                maxHeight: $(window).height(),
                maxWidth: $(window).width(),
                height: ($(window).height() / 10) * 9,
                width: ($(window).width() / 10) * 9,           
                modal: true,
                visible: false,
                resizable: true,
                scrollable: true,
                actions: ["print","download", "Minimize", "Maximize", "Close"],
                close: function() {
                    saveScreenInfo();
                    updateLoadSummaryData();
                    //btnCalculateLoad_Click();
                }
            }).data("kendoWindow");
          
            //var winmaxItemsWidth = ($(window).width() / 10) * 9;
            //var winmaxItemsHeight = ($(window).height() / 10) * 9;
            //height: ($(window).height() / 10) * 8,
            //width: ($(window).width() / 10) * 8,
            //winItems.setOptions({
            //    maxHeight: $(window).height(),
            //    maxWidth: $(window).width(),
            //    height: ($(window).height() / 10) * 9,
            //    width: ($(window).width() / 10) * 9
            //});
            //if (blnwinItemsEventAdded === 0) {

            //$("#winDispatchDialog").data("kendoWindow").wrapper.find(".k-svg-i-upload").parent().click(function (e) { e.preventDefault(); tObj.add(); });
            winItems.wrapper.find(".k-svg-i-download").parent().click(function (e) {                    
                //debugger;                   
                //alert('winItems Download click');
                e.preventDefault();
                CloseWinItemAndCalculate();                 
            });               
            winItems.wrapper.find(".k-svg-i-print").parent().click(function (e) {                   
                //debugger;                  
                //alert('winItems print click');
                e.preventDefault(); 
                printwinItems();               
            });
            //}
            //blnwinItemsEventAdded = 1;

            blnWinItemsClosing = false;
            winItems.center().open();
        }

        function btnManageAccessorials_Click(){
            //winAccessorials.center().open();
        }

        function openZipWin(City, State) {
            //load content
            $("#txtZipCity").val(City);
            $("#txtZipState").val(State);          
            $("#comboZip").data("kendoComboBox").dataSource.read();
            zipWin.center().open();
        }

        function btnZipOk_Click() {
            var combobox = $("#comboZip").data("kendoComboBox");
            var dataItem = combobox.dataItem();
            var zip;
            if (dataItem == null){ zip = $("#comboZip").data("kendoComboBox").value(); } else{ zip = dataItem.ZipCode; }
            var l = $("#txtZipLocation").val();
            if (l == "o"){ $("#txtorigCompPostalCode").data("kendoComboBox").value(zip); }
            if (l == "d"){ $("#txtdestCompPostalCode").data("kendoComboBox").value(zip); }       
            combobox.value("");
            $("#txtZipCity").val("");
            $("#txtZipState").val("");
            zipWin.close();
        }
         
        function expandOrderInfo() { $("#divOrderInfo").show(); $("#ExpandOrderInfoSpan").hide(); $("#CollapseOrderInfoSpan").show(); }
        function collapseOrderInfo() { $("#divOrderInfo").hide(); $("#ExpandOrderInfoSpan").show(); $("#CollapseOrderInfoSpan").hide(); }
        function expandCurrentItems() { $("#divCurrentItems").show(); $("#ExpandCurrentItemsSpan").hide(); $("#CollapseCurrentItemsSpan").show(); }
        function collapseCurrentItems() { $("#divCurrentItems").hide(); $("#ExpandCurrentItemsSpan").show(); $("#CollapseCurrentItemsSpan").hide(); }
        function expandAccessorials() { $("#divAccessorials").show(); $("#ExpandAccessorialsSpan").hide(); $("#CollapseAccessorialsSpan").show(); }
        function collapseAccessorials() { $("#divAccessorials").hide(); $("#ExpandAccessorialsSpan").show(); $("#CollapseAccessorialsSpan").hide(); }
        function expandPopUpSummary() { $("#divPopUpSummary").show(); $("#ExpandPopUpSummarySpan").hide(); $("#CollapsePopUpSummarySpan").show(); }
        function collapsePopUpSummary() { $("#divPopUpSummary").hide(); $("#ExpandPopUpSummarySpan").show(); $("#CollapsePopUpSummarySpan").hide(); }
        
        function fnGetPickup()
        {
            var stop = new rateReqStop();             
            stop.StopIndex = 1;
            stop.CompControl = $("#txtorigCompControl").val(); 
            stop.CompName = $("#txtorigCompName").data("kendoMaskedTextBox").value();
            stop.CompAddress1 = $("#txtorigCompAddress1").data("kendoMaskedTextBox").value();
            stop.CompAddress2 = $("#txtorigCompAddress2").val(); 
            stop.CompAddress3 = $("#txtorigCompAddress3").val();                 
            stop.CompCountry = $("#txtorigCompCountry").data("kendoMaskedTextBox").value(); 
            stop.CompPostalCode = getOrigPC();
            stop.CompCity = getOrigCity();
            stop.CompState = getOrigST();
            stop.IsPickup = true; 
            stop.StopNumber = 0;
            stop.TotalCases = $("#txtTotalCases").data("kendoNumericTextBox").value(); 
            stop.TotalWgt = $("#txtTotalWgt").data("kendoNumericTextBox").value(); 
            stop.TotalPL = $("#txtTotalPlts").data("kendoNumericTextBox").value(); 
            stop.TotalCube = 0;
            stop.LoadDate = $("#txtShipDate").data("kendoDatePicker").value();
            stop.RequiredDate  = $("#txtDeliveryDate").data("kendoDatePicker").value();
            stop.WeightUnit  = $("#txtWeightUnit").data("kendoComboBox").value();
            stop.LengthUnit  = $("#txtLengthUnit").data("kendoComboBox").value(); 
            return stop;
        }

        function fnGetStops()
        {
            //debugger;
            orderstops = new Array();           
            var stop = new rateReqStop();                            
            stop.SHID = $("#txtSHID").data("kendoMaskedTextBox").value();
            stop.CompAddress1 = $("#txtdestCompAddress1").data("kendoMaskedTextBox").value();
            stop.CompAddress2 = $("#txtdestCompAddress2").val(); 
            stop.CompAddress3 = $("#txtdestCompAddress3").val();  
            stop.CompCountry = $("#txtdestCompCountry").data("kendoMaskedTextBox").value();
            stop.CompPostalCode = getDestPC();
            stop.CompCity = getDestCity();
            stop.CompState = getDestST();       
            stop.RequiredDate = $("#txtDeliveryDate").data("kendoDatePicker").value();                                     
            stop.StopIndex = 1;
            stop.CompControl = isNull(parseInt($("#txtBookCustCompControl").val()),0); //** NEVER GETS USED **
            stop.CompName = $("#txtdestCompName").data("kendoMaskedTextBox").value(); 
            stop.IsPickup = false; 
            stop.StopNumber = 1;           
            stop.LoadDate = $("#txtShipDate").data("kendoDatePicker").value();
            stop.TotalWgt = isNull(parseInt($("#txtTotalWgt").data("kendoNumericTextBox").value()),500);
            stop.TotalPL = isNull(parseInt($("#txtTotalPlts").data("kendoNumericTextBox").value()),1);
            stop.TotalCases = isNull(parseInt($("#txtTotalCases").data("kendoNumericTextBox").value()),1);        
            stop.WeightUnit  = $("#txtWeightUnit").data("kendoComboBox").value();
            stop.LengthUnit  = $("#txtLengthUnit").data("kendoComboBox").value(); 
            stop.TotalCube = 0;

            if( orderitems == null || orderitems.length < 1){
                stop.Items = new Array();
                var item1 = new rateReqItem();          
                item1.ItemIndex = 1;
                item1.LoadID = $("#txtLoadID").val(); 
                item1.ItemStopIndex = 1;
                item1.ItemNumber = 1;  
                item1.Weight = dWeight;
                item1.FreightClass = sFreightClass;
                item1.PalletCount = 1; 
                item1.NumPieces = 1; 
                item1.Description = "Desc"; 
                item1.Quantity = "1"; 
                item1.HazmatId = "";                                     
                item1.Code = ""; 
                item1.HazmatClass = ""; 
                item1.IsHazmat = false; 
                item1.Pieces = "";
                item1.PackageType = sPkgType;                 
                item1.Length = iPltLength; 
                item1.Width = iPltWidth; 
                item1.Height = iPltHeight; 
                item1.Density = ""; 
                item1.NMFCItem = ""; 
                item1.NMFCSub = "";
                item1.Stackable = null;

                stop.Items.push(item1);

                $("#txtTotalWgt").data("kendoNumericTextBox").value(500);
            }
            else{ stop.Items = orderitems; }
            
            orderstops.push(stop);
            return orderstops;
        }

        //LVV ADD
        function fnGetAccessorials(){
            var retfees = new Array();
            for (index = 0; index <  orderAccessorials.length; ++index){
                var fee = orderAccessorials[index];
                retfees.push(fee.Code);
            }
            return retfees;
        }

        function validateRequiredFields(){
            alertMsg = ""; //clear value
            var ret = true;            
            var fields = "";
            var strSp = "";                
            if (ngl.isNullOrWhitespace($("#txtorigCompCountry").data("kendoMaskedTextBox").value())){ fields += (strSp + "Orig Country"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtorigCompPostalCode").data("kendoComboBox").value())){ fields += (strSp + "Orig Zip"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtdestCompCountry").data("kendoMaskedTextBox").value())){ fields += (strSp + "Dest Country"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtdestCompPostalCode").data("kendoComboBox").value())){ fields += (strSp + "Dest Zip"); strSp = ", "; }
            if (!ngl.isNullOrWhitespace(fields)){ alertMsg = fields; ret = false; }
            return ret;
        }

        
        function GetRateReqOrder(){
            var order = new rateReqOrder(); 
            order.ID = $("#txtOrderID").val(); //** Reference to BookControl if availavle else does not get used **
            order.ShipKey = $("#txtSHID").val(); //** Reference to BookSHID if availavle else does not get used **
            order.BookConsPrefix  =   $("#txtBookConsPrefix").val(); //** Reference to BookConsPrefix if availavle else does not get used :  Previously this was mapped to  $("#txtSHID").data("kendoMaskedTextBox").value();
            order.ShipDate = $("#txtShipDate").data("kendoDatePicker").value();  //"02-25-2017";
            order.DeliveryDate = $("#txtDeliveryDate").data("kendoDatePicker").value();  //"03-06-2017";
            order.BookCustCompControl = $("#txtBookCustCompControl").val();  //** NEVER GETS USED **
            order.CompName = $("#txtorigCompName").data("kendoMaskedTextBox").value();
            order.CompNumber = $("#txtCompNumber").val(); //** NEVER GETS USED **
            order.CompAlphaCode = $("#txtCompAlphaCode").val(); //** NEVER GETS USED **
            order.BookCarrierControl = $("#txtBookCarrierControl").val(); //** NEVER GETS USED **
            order.CarrierName = $("#txtCarrierName").val(); //** NEVER GETS USED **
            order.CarrierNumber = $("#txtCarrierNumber").val(); //** NEVER GETS USED **
            order.CarrierAlphaCode = $("#txtCarrierAlphaCode").val(); //** NEVER GETS USED **
            order.TotalCases = $("#txtTotalCases").data("kendoNumericTextBox").value(); 
            order.TotalWgt = $("#txtTotalWgt").data("kendoNumericTextBox").value();
            order.TotalPL = $("#txtTotalPlts").data("kendoNumericTextBox").value();
            //Begin Modified by RHR for v-8.5.4.001 add temperature
            var tempCode = 'Dry';
            var diTmp = $("#ddlLoadTemp").data("kendoDropDownList").dataItem();
            if (diTmp != null) { tempCode = diTmp.Name; } 
            order.CommCodeDescription = tempCode;
            var tempType = 1;
            if (diTmp != null) { tempType = diTmp.Control; }
            order.TariffTempType = tempType;
            //End Modified by RHR for v-8.5.4.001 add temperature
            //TotalCube
            var labelText = $('#txtTotalStops').text();
            order.TotalStops = parseInt(labelText); 
            order.Pickup = fnGetPickup();
            order.Stops = fnGetStops();
            order.Accessorials = fnGetAccessorials();   //LVV ADD
            order.WeightUnit = $("#txtWeightUnit").data("kendoComboBox").value();
            order.LengthUnit = $("#txtLengthUnit").data("kendoComboBox").value(); 
            return order;            
        }
        
        function CloseWinItemAndCalculate(){
            //updateLoadSummaryData();
            if (blnWinItemsClosing == true) { return;}
            blnWinItemsClosing = true;
            winItems.close();
            btnCalculateLoad_Click();
        }

       
        function btnCalculateLoad_Click() {
            //var cp = $("#center-pane")
            //alert(cp.scrollTop().toString())
            //alert("position: " + cp.position().top.toString())
            //var top = cp.position().top;
            //var currentScroll = cp.scrollTop();
           
            //make sure the grid is not in edit mode
            var tf = $('#Items').find('.k-grid-edit-row');
            if (tf.length > 0){
                kendo.ui.progress($(document.body), false);
                ngl.showErrMsg("Unsaved Changes", "Cannot get rates while item grid is in edit mode", null);
                return;
            }
            //make sure all required fields are populated
            if (!validateRequiredFields()) {
                kendo.ui.progress($(document.body), false);
                ngl.showErrMsg("Required Fields", alertMsg, null);
                return;
            }       
            $("#center-pane").animate({ scrollTop:0 }, 500);
            kendo.ui.progress($(document.body), true);
            setTimeout(function(){
                var tObj = this;
                var order = GetRateReqOrder();
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("P44RateQuote/PostRateRequest", order, tObj, "PostRateRequestCallback", "PostRateRequestAjaxErrorCallback");
            }, 1000);
        }

        function GetRateRequestItems(LoadTenderControl) {        
            var tObj = this;
            var filter = new AllFilter();
            filter.filterName = 'LoadID';
            filter.filterValue = LoadTenderControl;
            filter.filterFrom = '';
            filter.filterTo = ''
            filter.sortName = 'ID';
            filter.sortDirection = 'ASC';
            filter.page = 1;
            filter.skip = 0;
            filter.take = 50;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.filteredRead("P44RateQuote/GetRateRequestItems", filter, tObj, "GetRateRequestItemsCallback", "GetRateRequestItemsAjaxErrorCallback");                 
        }

        function GetLoadTenderFees(LoadTenderControl) {                               
            var tObj = this;
            clearAccessorials_Click();
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("Accessorials/GetAccessorialsByLoadTender", LoadTenderControl, tObj, "GetLoadTenderFeesCallback", "GetLoadTenderFeesAjaxErrorCallback");
         }
                
        function OpenRateQuoteDetailsInit(e) {
            var eRateAdjustments = [];
            var eRateErrors = [];
            var eRateAdjustmentDataSource = kendo.data.DataSource;
            var eRateErrorsDataSource = kendo.data.DataSource;
            var detailRow = e.detailRow;
            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });
            if (e.data.AdjdescriptionCode1) { eRateAdjustments.push(new RateAdjustment("One", e.data.AdjfreightClass1, e.data.Adjweight1, e.data.Adjdescription1, e.data.AdjdescriptionCode1, e.data.Adjamount1, e.data.Adjrate1)); }
            if (e.data.AdjdescriptionCode2) { eRateAdjustments.push(new RateAdjustment("Two", e.data.AdjfreightClass2, e.data.Adjweight2, e.data.Adjdescription2, e.data.AdjdescriptionCode2, e.data.Adjamount2, e.data.Adjrate2)); }
            if (e.data.AdjdescriptionCode3) { eRateAdjustments.push(new RateAdjustment("Three", e.data.AdjfreightClass3, e.data.Adjweight3, e.data.Adjdescription3, e.data.AdjdescriptionCode3, e.data.Adjamount3, e.data.Adjrate3)); }
            if (e.data.AdjdescriptionCode4) { eRateAdjustments.push(new RateAdjustment("Four", e.data.AdjfreightClass4, e.data.Adjweight4, e.data.Adjdescription4, e.data.AdjdescriptionCode4, e.data.Adjamount4, e.data.Adjrate4)); }
            if (e.data.AdjdescriptionCode5) { eRateAdjustments.push(new RateAdjustment("Five", e.data.AdjfreightClass5, e.data.Adjweight5, e.data.Adjdescription5, e.data.AdjdescriptionCode5, e.data.Adjamount5, e.data.Adjrate5)); }             
            eRateAdjustmentDataSource = new kendo.data.DataSource({
                data: eRateAdjustments,
            });
            if (e.data.errorCode1) { eRateErrors.push(new RateErrors("One", e.data.errorCode1, e.data.errorMessage1, e.data.eMessage1, e.data.errorfieldName1)); }
            if (e.data.errorCode2) { eRateErrors.push(new RateErrors("Two", e.data.errorCode2, e.data.errorMessage2, e.data.eMessage2, e.data.errorfieldName2)); }
            if (e.data.errorCode3) { eRateErrors.push(new RateErrors("Three", e.data.errorCode3, e.data.errorMessage3, e.data.eMessage3, e.data.errorfieldName3)); }
            if (e.data.errorCode4) { eRateErrors.push(new RateErrors("Four", e.data.errorCode4, e.data.errorMessage4, e.data.eMessage4, e.data.errorfieldName4)); }
            if (e.data.errorCode5) { eRateErrors.push(new RateErrors("Five", e.data.errorCode5, e.data.errorMessage5, e.data.eMessage5, e.data.errorfieldName5)); }
            eRateErrorsDataSource = new kendo.data.DataSource({
                data: eRateErrors,
            });
            detailRow.find(".adjustments").kendoGrid({
                dataSource: eRateAdjustmentDataSource,
                resizeable: true,
                scrollable: true,
                sortable: false,
                pageable: false,
                columns: [
                    { field: "index", title: " ", width: 35 },
                    { field: "freightClass", title: "Class", width: 50 },
                    { field: "weight", title: "Wgt.", width: 50 },
                    { field: "description", title: "Desc.", width: 200 },
                    { field: "descriptionCode", title: "Code", width: 50 },
                    { field: "amount", title: "Amt.", width: 50 },
                    { field: "rate", title: "Rate", width: 50 }
                ]
            });
            detailRow.find(".rateerrors").kendoGrid({
                dataSource: eRateErrorsDataSource,
                resizeable: true,
                scrollable: true,
                sortable: false,
                pageable: false,
                columns: [
                    { field: "index", title: " ", width: 35 },
                    { field: "errorCode", title: "Code", width: 50 },
                    { field: "errorMessage", title: "Msg", width: 100 },
                    { field: "eMessage", title: "Details", width: 100 },
                    { field: "errorfieldName", title: "Field Name", width: 75 }
                ]
            });
        }
        
        //function selectPkgeDesc_Click(){
        //    var grid =  $('#Items').data('kendoGrid');
        //    var selectedItem = grid.dataItem(grid.select());
        //    if (!selectedItem) {
        //        alert("Please select an Item to continue, if you are adding a new item you must save your changes then select the new row.")
        //    } else {
        //        alert(selectedItem.Description);
        //    }
           
        //}
        
        function clearItems_Click(){
            intItemNumIndex = 0;
            orderitems = new Array();
            $('#Items').data('kendoGrid').dataSource.read();
        }

        function clearAccessorials_Click(){
            intFeeIndex = 0;
            orderAccessorials = new Array();
            $('#Accessorials').data('kendoGrid').dataSource.read();
        }
           
        //LVV Change Accessorial
        ////function btnAddFee_Click() {           
        ////    var combobox = $("#comboAccessorials").data("kendoComboBox");
        ////    var dataItem = combobox.dataItem();
        ////    if (dataItem == null){
        ////        //alert("Fee dataItem is null");
        ////        return;
        ////    }
        ////    else {
        ////        intFeeIndex += 1;                
        ////        var fee = new NGLAPIAccessorial();                
        ////        $("#txtFeeIndex").val(intFeeIndex);
        ////        fee.FeeNumber = intFeeIndex;
        ////        fee.Control = dataItem.Control;
        ////        fee.Code = dataItem.Code;
        ////        fee.Name = dataItem.Name;
        ////        fee.Desc = dataItem.Desc;              
        ////        // save data item to the original datasource
        ////        orderAccessorials.push(fee);
        ////        $('#Accessorials').data('kendoGrid').dataSource.read();
        ////        $("#txtFeeIndex").val(0);
        ////    }      
        ////}

        function btnAddFee_Click() {           
            var combobox = $("#comboAccessorials").data("kendoDropDownList");
            var dataItem = combobox.dataItem();
            if (dataItem == null){
                //alert("Fee dataItem is null");
                return;
            }
            else {
                intFeeIndex += 1;                
                var fee = new NGLAPIAccessorial();                
                $("#txtFeeIndex").val(intFeeIndex);
                fee.FeeNumber = intFeeIndex;
                fee.Control = dataItem.Control;
                fee.Code = dataItem.Description;
                fee.Name = dataItem.Name;
                fee.Value = 0;             
                // save data item to the original datasource
                orderAccessorials.push(fee);
                $('#Accessorials').data('kendoGrid').dataSource.read();
                $("#txtFeeIndex").val(0);
            }      
        }//LVV Change Accessorial
                
        OrderItemsDataSource = new kendo.data.DataSource({
            transport: {
                read: function(e){e.success(orderitems);},
                create: function (e) {                    
                    intItemNumIndex += 1;
                    intItemIndex += 1;
                    var item1 = new rateReqItem();               
                    $("#txtItemIndex").val(intItemIndex);
                    item1.ItemIndex = intItemIndex;
                    item1.LoadID = $("#txtLoadID").val(); 
                    item1.ItemStopIndex = 1;
                    item1.ItemNumber = intItemNumIndex; 
                    item1.Weight = e.data.Weight;
                    item1.FreightClass = e.data.FreightClass;
                    item1.PalletCount = e.data.PalletCount; 
                    item1.NumPieces = 1;
                    item1.Description = e.data.Description;
                    item1.Quantity = e.data.Quantity; 
                    item1.HazmatId =  $("#txtHazmatId").val();                                     
                    item1.Code =  $("#txtCode").val(); 
                    item1.HazmatClass =  $("#txtHazmatClass").val(); 
                    item1.IsHazmat =  $("#txtIsHazmat").val(); 
                    item1.Pieces =  $("#txtPieces").val();
                    item1.PackageType =  e.data.PackageType; //$("#txtPackageType").val();                 
                    item1.Length =  e.data.Length; 
                    item1.Width =  e.data.Width; 
                    item1.Height =  e.data.Height; 
                    item1.Density =  $("#txtDensity").val(); 
                    item1.NMFCItem =  e.data.NMFCItem; //$("#txtNMFCItem").val(); //LVV Change 12/16/18
                    item1.NMFCSub =  e.data.NMFCSub; //$("#txNMFCSub").val(); //LVV Change 12/16/18
                    item1.Stackable = e.data.Stackable;
                    // save data item to the original datasource
                    orderitems.push(item1);
                    $("#txtItemIndex").val(0);
                    $("#txtLoadID").val(0);               
                    // on success
                    e.success(item1);
                    // on failure
                    //e.error("XHR response", "status code", "error message");
                },
                update: function(e){                   
                    // locate item in original datasource and update it
                    orderitems[getIndexByItemNumber(e.data.ItemNumber)] = e.data;
                    // on success
                    e.success();
                    // on failure
                    //e.error("XHR response", "status code", "error message");
                },
                destroy: function(e) {                   
                    orderitems.splice(getIndexByItemNumber(e.data.ItemNumber), 1);
                    e.success(orderitems);
                    // on failure
                    //e.error("XHR response", "status code", "error message");
                },
                parameterMap: function (options, operation) { return options; }
            },
            requestEnd: function(e) {               
                var index; 
                var TotalCases = 0;
                var TotalWgt = 0;
                var TotalPlts = 0;
                if(orderitems.length > 0){
                    for (index = 0; index < orderitems.length; ++index){
                        var item = orderitems[index];
                        TotalCases += parseInt(item.Quantity);
                        TotalWgt += parseFloat(item.Weight);
                        TotalPlts += parseFloat(item.PalletCount);                   
                    }
                }
                else{
                    TotalCases = 1;
                    TotalWgt = 1;
                    TotalPlts = 1; 
                }
                $("#txtTotalWgt").data("kendoNumericTextBox").value(TotalWgt);
                $("#txtTotalPlts").data("kendoNumericTextBox").value(TotalPlts);
                $("#txtTotalCases").data("kendoNumericTextBox").value(TotalCases);
            },
            schema: {
                model: {
                    id: "ItemIndex",
                    fields: {
                        "ItemIndex": { type: "number", visible: false, editable: false, nullable: true },
                        "ItemStopIndex": { type: "number", visible: false, editable: false, nullable: true },
                        "LoadID": { type: "number", visible: false, editable: false, nullable: true },
                        "ItemNumber": { type: "string", editable: false },
                        "Weight": { type: "number", defaultValue: dWeight, validation: { required: true, min: 1 } },
                        "WeightUnit": { type: "string", defaultValue: sWeightUnit, editable: false, nullable: true },
                        "FreightClass": { type: "string", defaultValue: sFreightClass, editable: true, nullable: false },
                        "PalletCount": { type: "number", defaultValue: 1, validation: { required: true, min: 1 } },
                        "NumPieces": { type: "number",  defaultValue: 1, validation: { required: true, min: 1 }},
                        "Description": { type: "string", editable: true, nullable: true },
                        "Quantity": { type: "string", defaultValue: 1, validation: { required: true, min: 1 } },
                        "HazmatId": { type: "string", editable: true, nullable: true },
                        "Code": { type: "string", editable: true, nullable: true },
                        "HazmatClass": { type: "string", editable: true, nullable: true },
                        "IsHazmat": { type: "boolean", editable: true, nullable: true },
                        "Pieces": { type: "string", defaultValue: "1",  editable: true, nullable: true },
                        "PackageType": { type: "string", defaultValue: sPkgType, editable: true, nullable: false },
                        "Length": { type: "number", defaultValue: iPltLength, validation: { required: true, min: 0 } },
                        "Width": { type: "number", defaultValue: iPltWidth, validation: { required: true, min: 0 } },
                        "Height": { type: "number", defaultValue: iPltHeight, validation: { required: true, min: 0 } },
                        "Density": { type: "string", editable: true, nullable: true },
                        "NMFCItem": { type: "string", editable: true, nullable: true },
                        "NMFCSub": { type: "string", editable: true, nullable: true },
                        "Stackable": { type: "boolean", editable: true, nullable: true },
                        "PackageDescription": { type: "number", defaultValue: 0, editable: true, nullable: false }
                    }
                }
            },
            error: function(e) {
                alert(e.errorThrown + " - " + e.xhr.responseJSON.ExceptionType + ":\n\n" + e.xhr.responseJSON.ExceptionMessage + "<br\>(Source: OrderItemsDataSource)"); 
                this.cancelChanges();
            },
        });

        function loadOrderStopsDataSource(){
            $("#Items").empty();     
            var ItemsGrid = $("#Items").kendoGrid({
                toolbar: [ 
                    { name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick="return openAddNewBookPkgGridWindow(event,0)"><span class="k-icon k-i-add"></span>Add</a>' },
                    { template: "<a class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='clearItems_Click();'><span class='k-icon k-i-minus'></span> Clear Items</a>" },
                ],
                selectable: "single, row",               
                scrollable: true,
                resizable: true,
                columns: [
                    {
                        command: [
                            { className: "cm-icononly-button", name: "EditBookPkgGridCRUDCtrl", text: "", iconClass: "k-icon k-i-pencil", click: openEditBookPkgGridWindow },
                            { className: "cm-icononly-button", name: "DeleteBookPkgGridCRUDCtrl", text: "", iconClass: "k-icon k-i-trash", click: confirmDeleteBookPkgGridRecord }],
                        title: "Actions", width: 90
                    },
                    { field: "ItemNumber", title: "Item Number", hidden: true },
                    { field: "Quantity", title: "Qty", hidden: true },
                    { field: "PackageType",title: "Package Type", width: 100 },
                    { field: "PalletCount", title: "Count" },                    
                    { field: "Description", title: "Desc", width: 200 },
                    { field: "FreightClass", title: "FAK", width: 100 },
                    { field: "NMFCItem", title: "NMFC" },
                    { field: "NMFCSub", title: "Sub Class" },
                    { field: "Weight", title: "Wgt" },                   
                    { field: "Length", title: "Length", width: 75 },
                    { field: "Width", title: "Width", width: 75 },
                    { field: "Height", title: "Height", width: 75 },
                    { field: "Stackable", title: "Stack", template: '<input type="checkbox" #= Stackable ? "checked=checked" : "" # disabled="disabled" ></input>', width: 75 }
                ],
                columnMenu: {columns: true},
                dataSource: OrderItemsDataSource,
            });

            $("#Accessorials").empty();
            OrderAccessorialsDataSource = new kendo.data.DataSource({
                transport: {
                    read: function(e){e.success(orderAccessorials);},
                    destroy: function(e) {
                        orderAccessorials.splice(getIndexByFeeNumber(e.data.FeeNumber), 1);
                        e.success(orderAccessorials);
                    },
                    update: function(e){                   
                        // locate item in original datasource and update it
                        orderitems[getIndexByItemNumber(e.data.FeeNumber)] = e.data;
                        // on success
                        e.success();
                        // on failure
                        //e.error("XHR response", "status code", "error message");
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    model: {
                        id: "FeeNumber",
                        fields: {
                            "FeeNumber": { type: "number", visible: true, editable: false, nullable: true },
                            "Control": { type: "number", visible: true, editable: false, nullable: true },
                            "Code": { type: "string", editable: true },
                            "Name": { type: "string", editable: true, nullable: true },
                            "Desc": { type: "string", editable: true, nullable: true },
                            "Value": { type: "number", visible: true, editable: true }
                        }
                    }
                },
                error: function(e) {
                    alert(e.errorThrown + " - " + e.xhr.responseJSON.ExceptionType + ":\n\n" + e.xhr.responseJSON.ExceptionMessage + "<br\>(Source: OrderAccessorialsDataSource)"); 
                    this.cancelChanges();
                },
            });

            AccessorialsGrid = $("#Accessorials").kendoGrid({
                toolbar: [ 
                    { name: "Custom", template: '<a class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md"  onclick="return openAddNewAccessorialsGridWindow(event,0)"><span class="k-icon k-i-add"></span>Add</a>' },
                    { template: "<a class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='clearAccessorials_Click();'><span class='k-icon k-i-minus'></span> Clear Accessorials</a>" },
                ], 
                selectable: "single, row",
                scrollable: true,
                resizable: true,
                columns: [
                    {
                        command: [
                            { className: "cm-icononly-button", name: "EditAccessorialsGridCRUDCtrl", text: "", iconClass: "k-icon k-i-pencil", click: openEditAccessorialsGridWindow },
                            { className: "cm-icononly-button", name: "DeleteAccessorialsGridCRUDCtrl", text: "", iconClass: "k-icon k-i-trash", click: confirmDeleteAccessorialsGridRecord }],
                        title: "Actions", width: 90
                    },                    
                    { field: "FeeNumber", title: "Fee Number", width: 75, hidden: true },
                    { field: "Control", title: "Control", width: 75, hidden: true },
                    { field: "Code", title: "Code", width: 75 },
                    { field: "Name", title: "Name", width: 75 },
                    { field: "Desc", title: "Description", width: 100},
                    { field: "Value", title: "Fee", format: "{0:c2}", width: 75 }
                ],
                columnMenu: {columns: true},                
                dataSource: OrderAccessorialsDataSource
            });
        }

        function btnClear_Click(){
            if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                kendo.ui.progress($(document.body), false);
                //oQuotesGrid.empty();
            }         
            intItemNumIndex = 0;
            intFeeIndex = 0;
            orderitems = new Array();
            orderstops = new Array();
            orderAccessorials = new Array();

            $('#Items').data('kendoGrid').dataSource.read();
            $('#Accessorials').data('kendoGrid').dataSource.read();

            $("#txtBookProNumber").val(''); 
            $("#txtBookControl").val(0); 
            $("#txtOrderID").val(0);  //** NEVER GETS USED **
            $("#txtBookCustCompControl").val(0); //** NEVER GETS USED **
            $("#txtCompNumber").val(0); 
            $("#txtCompAlphaCode").val(''); 
            $("#txtBookCarrierControl").val(0); 
            $("#txtCarrierName").val(''); 
            $("#txtCarrierNumber").val(0); 
            $("#txtCarrierAlphaCode").val(''); 
            $("#txtBookSHID").val(''); 
            $("#txtTotalStops").text("0"); 
            $("#txtorigCompControl").val(0); 
            $("#txtorigCompNumber").val(0); 
            $("#txtorigCompAddress2").val(''); 
            $("#txtorigCompAddress3").val(''); 
            $("#txtdestCompControl").val(0); 
            $("#txtdestCompNumber").val(''); 
            $("#txtdestCompAddress2").val(''); 
            $("#txtdestCompAddress3").val(''); 
            $("#txtStopIndex").val(0); 
            $("#txtSelectedStopIndex").val(0); 
            $("#txtItemIndex").val(0); 
            $("#txtLoadID").val(0); 
            $("#txtItemStopIndex").val(0); 
            $("#txtNumPieces").val(1); 
            $("#txtDescription").val(''); 
            $("#txtHazmatId").val(''); 
            $("#txtCode").val(''); 
            $("#txtHazmatClass").val(''); 
            $("#txtIsHazmat").val(0); 
            $("#txtPieces").val(0); 
            //$("#txtPackageType").val(''); 
            $("#txtDensity").val(0); 
            $("#txtNMFCItem").val(''); 
            $("#txNMFCSub").val(''); 
            $("#txtOrigPhone").val(''); 
            $("#txtOrigContactName").val(''); 
            $("#txtOrigContactEmail").val(''); 
            $("#txtOrigEmergencyContactPhone").val(''); 
            $("#txtOrigEmergencyContactName").val(''); 
            $("#txtDestPhone").val(''); 
            $("#txtDestContactName").val(''); 
            $("#txtDestContactEmail").val(''); 
            $("#txtDestEmergencyContactPhone").val(''); 
            $("#txtDestEmergencyContactName").val(''); 
            $("#txtShipDate").data("kendoDatePicker").value(ngl.formatTodayForkendoDatePicker());
            $("#txtDeliveryDate").data("kendoDatePicker").value(ngl.formatFutureDayForkendoDatePicker(7));
            $("#txtTotalWgt").data("kendoNumericTextBox").value(500);
            $("#txtTotalPlts").data("kendoNumericTextBox").value(1);
            $("#txtTotalCases").data("kendoNumericTextBox").value(1);     
            $("#txtTotalStops").text("0");
            $("#txtorigCompName").data("kendoMaskedTextBox").value('Ship From');
            $("#txtorigCompAddress1").data("kendoMaskedTextBox").value('');
            $("#txtorigCompCountry").data("kendoMaskedTextBox").value(sCountry);
            $("#txtorigCompPostalCode").data("kendoComboBox").value("");
            $("#txtorigCompCity").data("kendoComboBox").value("");
            $("#txtorigCompState").data("kendoComboBox").value("");   
            $("#txtdestCompName").data("kendoMaskedTextBox").value('Ship to');
            $("#txtdestCompAddress1").data("kendoMaskedTextBox").value('');
            $("#txtdestCompCountry").data("kendoMaskedTextBox").value(sCountry);
            $("#txtdestCompPostalCode").data("kendoComboBox").value("");
            $("#txtdestCompCity").data("kendoComboBox").value("");
            $("#txtdestCompState").data("kendoComboBox").value("");   
            $("#txtSHID").data("kendoMaskedTextBox").value('(Auto)');
            $("#txtFeeIndex").val(0);  
            $("#txtClass").data("kendoMaskedTextBox").value(sFreightClass);
            //debugger;
            $("#txtWeightUnit").data("kendoComboBox").value(sWeightUnit);
            $("#txtLengthUnit").data("kendoComboBox").value(sLengthUnit);
            //loadAddressBook();
            updateLoadSummaryData();
        }
        
        function isNull(val,def){
            var ret = 0;  
            if(isNaN(val)) {
                if (isNaN(def)) { def = 0;}
                ret = def;
            } else { ret = val; }
            return ret;
        }

        function getIndexByFeeNumber(feeNo) {
            var idx, l = orderAccessorials.length;
            for (var j=0; j < l; j++) {
                if (orderAccessorials[j].FeeNumber == feeNo) { return j; }
            }
            return null;
        }

        function getIndexByItemNumber(itemNo) {
            var idx, l = orderitems.length;
            for (var j=0; j < l; j++) {
                if (orderitems[j].ItemNumber == itemNo) { return j; }
            }
            return null;
        }
               

        function getOrigPC(){
            var dataItemZ = $("#txtorigCompPostalCode").data("kendoComboBox").dataItem();
            if (dataItemZ == null){ return $("#txtorigCompPostalCode").data("kendoComboBox").value(); } else{ return dataItemZ.ZipCode; }
        }
        function getOrigST(){
            var dataItemS = $("#txtorigCompState").data("kendoComboBox").dataItem();
            if (dataItemS == null){ return $("#txtorigCompState").data("kendoComboBox").value(); } else{ return dataItemS.State; }
        }
        function getOrigCity(){
            var dataItemC = $("#txtorigCompCity").data("kendoComboBox").dataItem();        
            if (dataItemC == null){ return $("#txtorigCompCity").data("kendoComboBox").value(); } else{ return dataItemC.City; } 
        }

        function getDestPC(){
            var dataItemZ = $("#txtdestCompPostalCode").data("kendoComboBox").dataItem();
            if (dataItemZ == null){ return $("#txtdestCompPostalCode").data("kendoComboBox").value(); } else{ return dataItemZ.ZipCode; }
        }
        function getDestST(){
            var dataItemS = $("#txtdestCompState").data("kendoComboBox").dataItem();
            if (dataItemS == null){ return $("#txtdestCompState").data("kendoComboBox").value(); } else{ return dataItemS.State; }
        }
        function getDestCity(){
            var dataItemC = $("#txtdestCompCity").data("kendoComboBox").dataItem();
            if (dataItemC == null){ return $("#txtdestCompCity").data("kendoComboBox").value(); } else{ return dataItemC.City; }
        }
        
        function GetOrigin()
        {
            var origin = new AddressBook();
            origin.Name = $("#txtorigCompName").data("kendoMaskedTextBox").value();
            origin.Address1 = $("#txtorigCompAddress1").data("kendoMaskedTextBox").value();
            origin.Address2 = $("#txtorigCompAddress2").val(); 
            origin.Address3 = $("#txtorigCompAddress3").val();
            origin.City = getOrigCity();
            origin.State = getOrigST();
            origin.Country = $("#txtorigCompCountry").data("kendoMaskedTextBox").value(); 
            origin.Zip = getOrigPC();          
            origin.Contact = GetOrigContact();                
            return origin;
        }

        function GetDest()
        {
            var dest = new AddressBook();            
            dest.Name = $("#txtdestCompName").data("kendoMaskedTextBox").value();
            dest.Address1 = $("#txtdestCompAddress1").data("kendoMaskedTextBox").value();
            dest.Address2 = $("#txtdestCompAddress2").val(); 
            dest.Address3 = $("#txtdestCompAddress3").val();  
            dest.City = getDestCity();
            dest.State = getDestST();
            dest.Country = $("#txtdestCompCountry").data("kendoMaskedTextBox").value();
            dest.Zip = getDestPC();
            dest.Contact = GetDestContact();                
            return dest;         
        }

        function GetItems(){
            //debugger;
            var retItems = new Array();
            if(orderitems != null && orderitems.length > 0){
                for (index = 0; index < orderitems.length; ++index){
                    var i = orderitems[index];
                    var item = new Item();
                    item.ItemNumber = i.ItemNumber;
                    item.ItemDesc = i.Description;
                    item.ItemFreightClass = i.FreightClass;
                    item.ItemWgt = i.Weight;
                    item.ItemPackageType = i.PackageType;
                    item.ItemTotalPackages = i.PalletCount;
                    item.ItemPieces = i.Quantity;                                
                    item.ItemStackable = i.Stackable;
                    item.ItemLength = i.Length;
                    item.ItemWidth = i.Width;
                    item.ItemHeight = i.Height;
                    item.ItemNMFCItemCode = i.NMFCItem; //""; //LVV Change 12/16/18
                    item.ItemNMFCSubCode = i.NMFCSub; //""; //LVV Change 12/16/18
                    item.ItemIsHazmat = false;
                    item.ItemHazmatID = "";
                    item.ItemHazmatClass = "";
                    item.ItemHazmatPackingGroup = "";
                    item.ItemHazmatProperShipName = "";

                    retItems.push(item);                   
                }                     
            }
            return retItems;
        }


        function GetAccessorials(){
            var arrayRet = new Array();
            if(orderAccessorials != null && orderAccessorials.length > 0){
                for (index = 0; index < orderAccessorials.length; ++index){
                    var a = orderAccessorials[index];
                    arrayRet.push(a.Code);
                }                     
            }
            return arrayRet;
        }
        
        function GetEmergencyContact(){
            var cont = new Contact();
            var OrigEmergencyContactPhone = $("txtOrigEmergencyContactPhone").val();
            if (typeof (OrigEmergencyContactPhone) !== 'undefined' && OrigEmergencyContactPhone !== null && !ngl.isNullOrWhitespace(OrigEmergencyContactPhone)) {
                cont.ContactControl = 0; //identifies this as the origin contact               
                cont.ContactPhone = $("#txtOrigEmergencyContactPhone").val();                   
                cont.ContactName = $("#txtOrigEmergencyContactName").val();                
            } else {
                var DestEmergencyContactPhone = $("txtDestEmergencyContactPhone").val();
                if (typeof (DestEmergencyContactPhone) !== 'undefined' && DestEmergencyContactPhone !== null && !ngl.isNullOrWhitespace(DestEmergencyContactPhone)) {
                    cont.ContactControl = -1; //identifies this as the destination contact
                    cont.ContactPhone = $("#txtDestEmergencyContactPhone").val();                   
                    cont.ContactName = $("#txtDestEmergencyContactName").val();
                } 
            }  
            return cont;  
        }

        function SetEmergencyContact(cont){
            if (typeof (cont) === 'undefined' || cont === null || !ngl.isObject(cont)) {return;}             
            if (cont.ContactControl === -1) {
                //identifies this as the destination contact
                $("#txtDestEmergencyContactName").val(cont.ContactName);
                $("#txtDestEmergencyContactPhone").val(cont.ContactPhone);
            } else {
                 $("#txtOrigEmergencyContactName").val(cont.ContactName);
                 $("#txtOrigEmergencyContactPhone").val(cont.ContactPhone);
            }            
        }

        function GetOrigContact(){
            var cont = new Contact();                        
            cont.ContactPhone = $("#txtOrigPhone").val();                   
            cont.ContactName = $("#txtOrigContactName").val();                              
            cont.ContactEmail = $("#txtOrigContactEmail").val();            
            return cont;  
        }

        function SetOrigContact(cont){
            if (typeof (cont) === 'undefined' || cont === null || !ngl.isObject(cont)) {return;}                       
            $("#txtOrigContactName").val(cont.ContactName);
            $("#txtOrigPhone").val(cont.ContactPhone); 
            $("#txtOrigContactEmail").val(cont.ContactEmail);
        }

        function GetDestContact(){
            var cont = new Contact();                        
            cont.ContactPhone = $("#txtDestPhone").val();                   
            cont.ContactName = $("#txtDestContactName").val();                              
            cont.ContactEmail = $("#txtDestContactEmail").val();            
            return cont;  
        }

        function SetDestContact(cont){
            if (typeof (cont) === 'undefined' || cont === null || !ngl.isObject(cont)) {return;}                        
            $("#txtDestContactName").val(cont.ContactName);
            $("#txtDestPhone").val(cont.ContactPhone);           
            $("#txtDestContactEmail").val(cont.ContactEmail);
        }
        
        var blnDispatchedFromPageQuote = false;

        function Dispatch_Click() {         
            if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {              
                oDispatchData = new Dispatch();        
                //Get the selected row from the grid
                var row = oQuotesGrid.select();
                if (typeof (row) === 'undefined' && row == null) { ngl.showErrMsg("Rate Quote Required", "Please select a Quote to Dispatch", null); return; }        
                //Get the dataItem for the selected row
                var dataItem = oQuotesGrid.dataItem(row);
                var providerSCAC = "";
                var vendorSCAC = "";
                var quote = "";
                if (typeof (dataItem) !== 'undefined' && dataItem != null) { 
                    if ('BidCarrierSCAC' in dataItem) { providerSCAC = dataItem.BidCarrierSCAC; }
                    if ('BidVendor' in dataItem) { vendorSCAC = dataItem.BidVendor; }
                    if ('BidQuoteNumber' in dataItem) { quote = dataItem.BidQuoteNumber; }
                } 
                else { ngl.showErrMsg("Rate Quote Required", "Please select a Quote to Dispatch", null); return; }
                oDispatchData.ProviderSCAC = providerSCAC;
                oDispatchData.VendorSCAC = vendorSCAC; 
                oDispatchData.QuoteNumber = quote; 
                if ('BidSHID' in dataItem) {oDispatchData.SHID = dataItem.BidSHID};
                if ('BidControl' in dataItem) {oDispatchData.BidControl = dataItem.BidControl};              
                //d.ProNumber = ""; 
                oDispatchData.LoadTenderControl = dataItem.BidLoadTenderControl;  // temporary place holder for the dispatch LoadTenderControl $("#txtSHID").data("kendoMaskedTextBox").value();
                //oDispatchData.OrderNumber = "";
                oDispatchData.SHID = $("#txtSHID").data("kendoMaskedTextBox").value();           
                oDispatchData.Origin = GetOrigin();
                oDispatchData.Destination = GetDest();
                oDispatchData.Requestor = oDispatchData.Origin;
                //d.Requestor = GetRequestor();
                //d.EmergencyContact = GetEmergencyCont();              
                oDispatchData.PickupDate = $("#txtShipDate").data("kendoDatePicker").value();
                oDispatchData.DeliveryDate = $("#txtDeliveryDate").data("kendoDatePicker").value();
                oDispatchData.TotalWgt = isNull(parseInt($("#txtTotalWgt").data("kendoNumericTextBox").value()),500);
                oDispatchData.TotalQty = isNull(parseInt($("#txtTotalCases").data("kendoNumericTextBox").value()),1);
                oDispatchData.TotalPlts = isNull(parseInt($("#txtTotalPlts").data("kendoNumericTextBox").value()),1);
                oDispatchData.TotalCube = 0;
                oDispatchData.WeightUnit = $("#txtWeightUnit").data("kendoComboBox").value();
                oDispatchData.LengthUnit = $("#txtLengthUnit").data("kendoComboBox").value(); 
                oDispatchData.LineHaul = dataItem.BidLineHaul;
                oDispatchData.Fuel = dataItem.BidFuelTotal;
                oDispatchData.TotalCost = dataItem.BidTotalCost;                                        
                oDispatchData.EmergencyContact = GetEmergencyContact();
                oDispatchData.Items = GetItems();
                oDispatchData.Accessorials = GetAccessorials();
                blnDispatchedFromPageQuote = true;            
                oDispatchingDialogCtrl.data = oDispatchData;
                oDispatchingDialogCtrl.show();
            }
        }


        function saveScreenInfo(){
            var s = new rateShopSaveLast();               
            s.OrigName = $("#txtorigCompName").data("kendoMaskedTextBox").value();
            s.OrigAddress1 = $("#txtorigCompAddress1").data("kendoMaskedTextBox").value();                          
            s.OrigCountry = $("#txtorigCompCountry").data("kendoMaskedTextBox").value(); 
            s.OrigCity = getOrigCity();
            s.OrigState = getOrigST();
            s.OrigPostalCode = getOrigPC();
            s.OrigContact = GetOrigContact();
            s.DestName = $("#txtdestCompName").data("kendoMaskedTextBox").value();
            s.DestAddress1 = $("#txtdestCompAddress1").data("kendoMaskedTextBox").value();
            s.DestCountry = $("#txtdestCompCountry").data("kendoMaskedTextBox").value();
            s.DestCity = getDestCity();          
            s.DestState = getDestST();
            s.DestPostalCode = getDestPC();
            s.DestContact = GetDestContact();
            s.SHID = $("#txtSHID").data("kendoMaskedTextBox").value();
            s.TotalCases = $("#txtTotalCases").data("kendoNumericTextBox").value(); 
            s.TotalWgt = $("#txtTotalWgt").data("kendoNumericTextBox").value(); 
            s.TotalPL = $("#txtTotalPlts").data("kendoNumericTextBox").value(); 
            s.TotalCube = 0;
            s.LoadDate = $("#txtShipDate").data("kendoDatePicker").value();
            s.RequiredDate  = $("#txtDeliveryDate").data("kendoDatePicker").value();
            s.WeightUnit    = $("#txtWeightUnit").data("kendoComboBox").value();
            s.LengthUnit = $("#txtLengthUnit").data("kendoComboBox").value(); 
            s.Items = orderitems;
            s.Accessorials = orderAccessorials;
            s.EmergencyContact = GetEmergencyContact();  
            s.TariffTempType = $("#ddlLoadTemp").data("kendoDropDownList").value(); //Modified by RHR for v-8.5.4.001 add temperature
            s.fakClass = $("#txtClass").data("kendoMaskedTextBox").value();

            var UserPageSettingsData = new PageSettingModel();                          
            UserPageSettingsData.name = "RateShop_SavedLastOrder";
            UserPageSettingsData.value = JSON.stringify(s);
            postPageSetting(tPage, "RateShopping", UserPageSettingsData, true)
        }

        function loadScreenInfo(){
            getPageSettings(tPage, "RateShopping", "RateShop_SavedLastOrder", true);
        }

        function updateLoadSummaryData() {
            var orig = $("#spOrigin");
            var sVal = "";
            sVal = sVal.concat($("#txtorigCompName").val(), '<br />', $("#txtorigCompAddress1").val(), '<br />', $("#txtorigCompCity").val(), " ", $("#txtorigCompState").val(), "  ", getOrigPC());
            orig.html(sVal);
            sVal = "";
            sVal = sVal.concat($("#txtorigCompCity").val(), " ", $("#txtorigCompState").val(), "  ", getOrigPC());
            $("#sppopupOrigin").html(sVal);
            var details = $("#spShipDetails");
            //sVal  = "Load: " + $("#txtShipDate").data("kendoDatePicker").value(); + '<br />' + "Deliver: " + $("#txtDeliveryDate").data("kendoDatePicker").value();
            //sVal  = string.concat("Load: ",$("#txtShipDate").val(),'<br />',"Deliver: ",$("#txtDeliveryDate").val());
            sVal = "";
            //sVal = sVal.concat("Load: ",$("#txtShipDate").val(),'  ',"Deliver: ",$("#txtDeliveryDate").val(),'<br />',"Weight: ", $("#txtTotalWgt").val(), "  ", "Plts: ", $("#txtTotalPlts").val());
            sVal = sVal.concat("To Load: ", $("#txtShipDate").val(), '<br />', "To Deliver: ", $("#txtDeliveryDate").val());
            details.html(sVal);
            sVal = "";
            sVal = sVal.concat("To Load: ", $("#txtShipDate").val(), " To Deliver: ", $("#txtDeliveryDate").val());
            $("#sppopupShipDetails").html(sVal);
            var shipWgt = $("#spShipWgt").html($("#txtTotalWgt").val());
            var shipWgt = $("#spShipQty").html($("#txtTotalCases").val());
            var shipWgt = $("#spShipPlts").html($("#txtTotalPlts").val());
            var shipTemp = $("#spShipTemperature").html($("#ddlLoadTemp").data("kendoDropDownList").value());
            var shipWgt = $("#sppopupShipWgt").html($("#txtTotalWgt").val());
            var shipWgt = $("#sppopupShipQty").html($("#txtTotalCases").val());
            var shipWgt = $("#sppopupShipPlts").html($("#txtTotalPlts").val());
            var shipTemp = $("#sppopupShipTemperature").html($("#ddlLoadTemp").data("kendoDropDownList").value());

            var dest = $("#spDest");
            sVal = "";
            sVal = sVal.concat($("#txtdestCompName").val(), '<br />', $("#txtdestCompAddress1").val(), '<br />', $("#txtdestCompCity").val(), " ", $("#txtdestCompState").val(), "  ", getDestPC());
            //sVal = string.concat($("#txtdestCompName").data("kendoMaskedTextBox").value(),'<br />',getDestPC());
            dest.html(sVal);
            sVal = "";
            sVal = sVal.concat($("#txtdestCompCity").val(), " ", $("#txtdestCompState").val(), "  ", getDestPC());
            $("#sppopupDest").html(sVal);
        }


        // this function toggles the BidSelectedForExport flag for the selected quote record
        function PostSelectQuote(e) {
            e.preventDefault();
            //// get the row with the button 
            var oRow = $(e.target).closest("tr"); // get the current table row (tr)
            // get the data bound to the current table row
            var data = this.dataItem(oRow);
            var iRowBidControl = data.BidControl;
            var blnSelectBid = data.BidSelectedForExport
            if (data.BidSelectedForExport == null || data.BidSelectedForExport == 0) {
                if (iRowBidControl != 0 ) {                    
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("tblBid/SelectBidForExport", iRowBidControl.toString(), tPage, "PostSelectQuoteSuccessCallback", "PostSelectQuoteAjaxErrorCallback", false);
                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            } else {
                if (iRowBidControl != 0) {                  
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("tblBid/UnSelectBidForExport", iRowBidControl.toString(), tPage, "PostSelectQuoteSuccessCallback", "PostSelectQuoteAjaxErrorCallback", false);

                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            }

        }

        // PostSelectQuote Call backs
        function PostSelectQuoteSuccessCallback(data) {
            kendo.ui.progress($("#center-pane"), false);
            if (ngl.coreSuccessCallBack(data, "Update Selected Rate for Export flag") == true) {
                if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                    kendo.ui.progress($(document.body), true);
                    ngl.readDataSource(oQuotesGrid);
                }
            } 
        }

        function PostHoldBookingAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($("#center-pane"), false);
            ngl.coreErrorCallBack(xhr, textStatus, error, "Update Selected Rate for Export flag");
        }


         // this function sets all of the BidSelectedForExport flags to true for all of the filtered quotes
        function selectAllQuotes(e) {
            if (!ngl.isNullOrWhitespace(sBidLoadTenderControlVal)) {
                kendo.ui.progress($("#center-pane"), true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.filteredPost("tblBid/SelectAllBidForExport", sBidLoadTenderControlVal.toString(), tPage, "PostSelectAllQuoteSuccessCallback", "PostSelectAllQuoteAjaxErrorCallback", false);
            } else { 
                ngl.showValidationMsg("Valid Bids are Required", "Please Get Rates and try again", tPage);
            }          
        }
         // this function set all of the BidSelectedForExport flags to false for all of the filtered quotes
        function unSelectAllQuotes(e) {
            if (!ngl.isNullOrWhitespace(sBidLoadTenderControlVal)) {
                kendo.ui.progress($("#center-pane"), true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.filteredPost("tblBid/UnSelectAllBidForExport", sBidLoadTenderControlVal.toString(), tPage, "PostSelectAllQuoteSuccessCallback", "PostSelectAllQuoteAjaxErrorCallback", false);
            } else {
                ngl.showValidationMsg("Valid Rates are Required", "Please Get Rates and try again", tPage);
            }   
        }


        // selectAllQuotes Call backs
        function PostSelectAllQuoteSuccessCallback(data) {
            kendo.ui.progress($("#center-pane"), false);
            if (ngl.coreSuccessCallBack(data, "Update All Rates Export flag") == true) {
                if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                    kendo.ui.progress($(document.body), true);
                    ngl.readDataSource(oQuotesGrid);
                }
            }
        }
        function PostSelectAllQuoteAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($("#center-pane"), false);
            ngl.coreErrorCallBack(xhr, textStatus, error, "Update All Rates for Export flag");
        }

         // this function set all of the BidSelectedForExport flags to true (Green) for all of the bids assigned to the filtered quote history records
         // if one of the bids is false the check box on the quote history summary records is Yellow
        function selectAllHistQuotes(e) {
            if (!ngl.isNullOrWhitespace(sHistQuotesGridKey)) {
                var sfilterFuncionname = "obj" + sHistQuotesGridKey + "Filters"                
                var s = window[sfilterFuncionname].data;               
                kendo.ui.progress($("#center-pane"), true);              
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.filteredRead("HistoricalQuotes/SelectAllBidForExportForAllHistoricalQuotes", s, tPage, "selectAllHistQuotesSuccessCallback", "selectAllHistQuotesAjaxErrorCallback", false);
            } else {
                ngl.showValidationMsg("Valid Quotes are Required", "Please Get Some Rates and try again", tPage);
            }  
        }
        // this function set all of the BidSelectedForExport flags to false (Red) for all of the bids assigned to the filtered quote history records
         // if one or more of the filtered bids BidSelectedForExport flag is true the check box on the quote history is Yellow
        function unSelectAllHistQuotes(e) {
            if (!ngl.isNullOrWhitespace(sHistQuotesGridKey)) {
                var sfilterFuncionname = "obj" + sHistQuotesGridKey + "Filters"  
                var s = window[sfilterFuncionname].data;
                kendo.ui.progress($("#center-pane"), true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.filteredRead("HistoricalQuotes/UnSelectAllBidForExportForAllHistoricalQuotes", s, tPage, "selectAllHistQuotesSuccessCallback", "selectAllHistQuotesAjaxErrorCallback", false);

            } else {
                ngl.showValidationMsg("Valid Quotes are Required", "Please Get Some Rates and try again", tPage);
            }  
        }

        // selectAllQuotes Call backs
        function selectAllHistQuotesSuccessCallback(data) {
            kendo.ui.progress($("#center-pane"), false);
            if (ngl.coreSuccessCallBack(data, "Update All Quotes and Rates Export flag") == true) {
                if (typeof (oQuotesGrid) !== 'undefined' && ngl.isObject(oQuotesGrid)) {
                    kendo.ui.progress($(document.body), true);
                    ngl.readDataSource(oHistQuotesGrid);
                }
            }
        }

        function selectAllHistQuotesAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($("#center-pane"), false);
            ngl.coreErrorCallBack(xhr, textStatus, error, "Update All Quotes and Rates Export flag");
        }
     

        // this function toggles the BidSelectedForExport flag for all of the bids for the selected quote history record
        function PostSelectHistQuote(e) {
            e.preventDefault();
            //// get the row with the button 
            var oRow = $(e.target).closest("tr"); // get the current table row (tr)
            // get the data bound to the current table row
            var data = this.dataItem(oRow);
            var iRowLoadTenderControl = data.LoadTenderControl;
            var blnSelectBid = data.LTSelectedForExport
            if (data.LTSelectedForExport == null || data.LTSelectedForExport == 0) {
                if (iRowLoadTenderControl != 0) {
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("HistoricalQuotes/SelectAllBidForExportForHistoricalQuote", iRowLoadTenderControl.toString(), tPage, "PostSelectHistQuoteSuccessCallback", "PostSelectHistQuoteAjaxErrorCallback", false);
                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            } else {
                if (iRowLoadTenderControl != 0) {
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("HistoricalQuotes/UnSelectAllBidForExportForHistoricalQuote", iRowLoadTenderControl.toString(), tPage, "PostSelectHistQuoteSuccessCallback", "PostSelectHistQuoteAjaxErrorCallback", false);

                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            }
        }

        // selectHistQuotes Call backs
        function PostSelectHistQuoteSuccessCallback(data) {
            kendo.ui.progress($("#center-pane"), false);
            if (ngl.coreSuccessCallBack(data, "Update All Rates Export flag") == true) {
                if (typeof (oHistQuotesGrid) !== 'undefined' && ngl.isObject(oHistQuotesGrid)) {
                    kendo.ui.progress($(document.body), true);
                    ngl.readDataSource(oHistQuotesGrid); 
                }
            }
        }
        function PostSelectHistQuoteAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($("#center-pane"), false);
            ngl.coreErrorCallBack(xhr, textStatus, error, "Update Quote Rates for Export flag");
        }

        // this function toggles the BidSelectedForExport flag for the selected quote history bid record
        function PostSelectHistQuoteBid(e) {
            e.preventDefault();
            //// get the row with the button 
            var oRow = $(e.target).closest("tr"); // get the current table row (tr)
            // get the data bound to the current table row
            var data = this.dataItem(oRow);
            var iRowBidControl = data.BidControl;
            var blnSelectBid = data.BidSelectedForExport
            if (data.BidSelectedForExport == null || data.BidSelectedForExport == 0) {
                if (iRowBidControl != 0) {
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("tblBid/SelectBidForExport", iRowBidControl.toString(), tPage, "PostSelectHistQuoteBidSuccessCallback", "PostSelectHistQuoteBidAjaxErrorCallback", false);
                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            } else {
                if (iRowBidControl != 0) {
                    kendo.ui.progress($("#center-pane"), true);
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.filteredPost("tblBid/UnSelectBidForExport", iRowBidControl.toString(), tPage, "PostSelectHistQuoteBidSuccessCallback", "PostSelectHistQuoteBidAjaxErrorCallback", false);

                } else {
                    ngl.showWarningMsg("Invalid row selection", "Please select a record and try again.", tPage);
                    return;
                }
            }
        }

        // selectHistQuotes Call backs
        function PostSelectHistQuoteBidSuccessCallback(data) {
            kendo.ui.progress($("#center-pane"), false);
            if (ngl.coreSuccessCallBack(data, "Update Selected Bid Export flag") == true) {
                if (typeof (oHistQuotesGrid) !== 'undefined' && ngl.isObject(oHistQuotesGrid)) {
                    kendo.ui.progress($(document.body), true);
                    ngl.readDataSource(oHistQuotesGrid);
                }
            }
        }

        function PostSelectHistQuoteAjaxErrorCallback(xhr, textStatus, error) {
            kendo.ui.progress($("#center-pane"), false);
            ngl.coreErrorCallBack(xhr, textStatus, error, "Update Selected Bid Export flag");
        }

        function exportSelectedHistQuotes() {
            if (!ngl.isNullOrWhitespace(sHistQuotesGridKey)) {
                sHistQuotesFilterFuncionName = "obj" + sHistQuotesGridKey + "Filters"
                sHistQuotesFilterData = window[sHistQuotesFilterFuncionName].data;
                

            $("#exportHistQuotesGrid").kendoGrid({
                autoBind: false,
                dataSource: getExportBidsDataSource(sHistQuotesFilterData),
                columns: [
                    { field: "BidCarrierName", title: "Carrier",  width: 75 },
                    { field: "LTBookDateLoad", title: "PICK UP DATE",  width: 75 },
                    { field: "LTBookOrigCity", title: "ORIGIN CITY",  width: 100 },
                    { field: "LTBookOrigZip", title: "ORIGIN POSTAL CODE",  width: 50 },
                    { field: "LTBookDestCity", title: "DEST CITY",  width: 100 },
                    { field: "LTBookDestZip", title: "DEST POSTAL CODE",  width: 50 },
                    { field: "LTBookTotalPL", fieldFormat: "{0:n0}", title: "PALLETS",  width: 50 },
                    { field: "LTBookTotalWgt", fieldFormat: "{0:n2}", title: "WEIGHT",  width: 50 },
                    { field: "LTCommCodeDescription", fieldFormat: "{0:n0}", title: "EQUIP",  width: 50 },
                    { field: "BidCustTotalCost", format: "{0:c2}", title: "Total Cost",  width: 50 },
                    { field: "BidCustLineHaul", format: "{0:c2}", title: "Line Haul",  width: 50 },
                    { field: "BidFuelTotal", format: "{0:c2}", title:  "Fuel",  width: 50 },
                    { field: "Fees", format: "{0:c2}", title: "Fees",  width: 50 },
                    { field: "BidQuoteDate", title: "Quote Date",  width: 75 },
                    { field: "BidQuoteNumber", title: "Quote Number",  width: 250 },
                    { field: "FreightClass", title: "Freight Class",  width: 50}  
                    ]
            });

                
                var grid = $("#exportHistQuotesGrid").data("kendoGrid");
             
               grid.saveAsExcel();
            } else {
                ngl.showValidationMsg("Valid Quotes are Required", "Please Get Some Rates and try again", tPage);
            } 
        }

        function exportSelectedQuotes() {
            //sBidLoadTenderControlVal
            if (!ngl.isNullOrWhitespace(sBidLoadTenderControlVal) && !isNaN(sBidLoadTenderControlVal)) {
                //alert("sBidLoadTenderControlVal: " + sBidLoadTenderControlVal);
                if (sBidLoadTenderControlVal == "0") {
                    ngl.showValidationMsg("Valid Quotes are Required", "Please Get Some Rates and try again", tPage);
                } else {                   

                    $("#exportQuotesGrid").kendoGrid({
                        autoBind: false,
                        dataSource: getdsExportQuotesDataSource(sBidLoadTenderControlVal),
                        columns: [
                            { field: "BidCarrierName", title: "Carrier", width: 75 },
                            { field: "LTBookDateLoad", title: "PICK UP DATE", width: 75 },
                            { field: "LTBookOrigCity", title: "ORIGIN CITY", width: 100 },
                            { field: "LTBookOrigZip", title: "ORIGIN POSTAL CODE", width: 50 },
                            { field: "LTBookDestCity", title: "DEST CITY", width: 100 },
                            { field: "LTBookDestZip", title: "DEST POSTAL CODE", width: 50 },
                            { field: "LTBookTotalPL", fieldFormat: "{0:n0}", title: "PALLETS", width: 50 },
                            { field: "LTBookTotalWgt", fieldFormat: "{0:n2}", title: "WEIGHT", width: 50 },
                            { field: "LTCommCodeDescription", fieldFormat: "{0:n0}", title: "EQUIP", width: 50 },
                            { field: "BidCustTotalCost", format: "{0:c2}", title: "Total Cost", width: 50 },
                            { field: "BidCustLineHaul", format: "{0:c2}", title: "Line Haul", width: 50 },
                            { field: "BidFuelTotal", format: "{0:c2}", title: "Fuel", width: 50 },
                            { field: "Fees", format: "{0:c2}", title: "Fees", width: 50 },
                            { field: "BidQuoteDate", title: "Quote Date", width: 75 },
                            { field: "BidQuoteNumber", title: "Quote Number", width: 250 },
                            { field: "FreightClass", title: "Freight Class", width: 50 }
                        ]
                    });

                    var grid = $("#exportQuotesGrid").data("kendoGrid");
                    grid.saveAsExcel();

                }
            }
        }


        //End page events

    $(window).on('beforeunload', function(){
        // your logic here
        if(control != 0){
            //saveScreenInfo();
        }      
    });

    $(document).on('focus', 'input[role="spinbutton"]', function () {
        $(this).select();
    });

     


    $(document).ready(function () {
        var PageMenuTab = <%=PageMenuTab%>;
        

        if (control != 0){
            setTimeout(function () {
                oDispatchingDialogCtrl = new DispatchingDialogCtrl(); 
                oDispatchingDialogCtrl.loadDefaults(winDispatchingDialog,oDispatchingDialogSelectCB,oDispatchingDialogSaveCB,oDispatchingDialogCloseCB,oDispatchingDialogReadCB);                   
                // oDispatchingDialogCtrl = new DispatchingDialogCtrl(); 
                //oDispatchingDialogCtrl.loadDefaults(winDispatchingDialog,oDispatchingDialogSelectCB,oDispatchingDialogSaveCB,oDispatchingDialogCloseCB,oDispatchingDialogReadCB);
                //setTimeout(function (oDispatchReportCtrl) {
                //    oDispatchReportCtrl = new DispatchingReportCtrl();             
                //    oDispatchReportCtrl.loadDefaults(winDispatchReport,oDispatchingReportSelectCB,oDispatchingReportSaveCB,oDispatchingReportCloseCB,oDispatchingReportReadCB);                     
                //}, 500, this);
                oDispatchReportCtrl = new DispatchingReportCtrl();           
                oDispatchReportCtrl.loadDefaults(winDispatchReport,oDispatchingReportSelectCB,oDispatchingReportSaveCB,oDispatchingReportCloseCB,oDispatchingReportReadCB);
                //setTimeout(function (oBOLReportCtrl) {
                //    oBOLReportCtrl = new BOLReportCtrl(); 
                //    oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);
                //}, 500, this);
                oBOLReportCtrl = new BOLReportCtrl();
                oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);
        
                $("#btnZipOk").kendoButton();
                $("#comboZip").kendoComboBox({
                    dataTextField: "ZipCode",
                    dataValueField: "ZipCode",
                    autoWidth: true,
                    autoBind: false,
                    template: '<span class="k-state-default"><p>#: data.ZipCode #</p></span>',
                    dataSource: {
                        transport: {
                            read: function(options) {
                                var z = new zip();
                                z.ZipCodeControl = 0;
                                z.ZipCode = "";
                                z.City = $("#txtZipCity").val();
                                z.State = $("#txtZipState").val();
                                $.ajax({
                                    async: false,
                                    type: "GET",
                                    url: "api/ZipCode/GetZipsForCityState",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    data: {filter: JSON.stringify(z)},
                                    success: function (data) { options.success(data); },
                                    error: function (xhr, textStatus, error) {
                                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Zip Code Failure");
                                        ngl.showErrMsg("Get Zip Code", sMsg, null);                        
                                    }
                                });
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                });
           
                zipWin = $("#zipWin").kendoWindow({               
                    title: "Choose a Zip Code",                
                    modal: true,
                    visible: false                        
                }).data("kendoWindow");

                //var winItemsWidth = ($(window).width() / 10) * 9;
                //var winItemsHeight = ($(window).height() / 10) * 9;
                //height: ($(window).height() / 10) * 8,
                //width: ($(window).width() / 10) * 8,
                //maxWidth: 800,
                winItems = $("#winItems").kendoWindow({
                    title: "Shipping Information",
                    maxHeight: $(window).height(),
                    maxWidth: $(window).width(),
                    height: ($(window).height() / 10) * 9,
                    width: ($(window).width() / 10) * 9,           
                    modal: true,
                    visible: false,
                    resizable: true,
                    scrollable: true,
                    actions: ["print","download", "Minimize", "Maximize", "Close"],
                    close: function() {
                        updateLoadSummaryData();
                        //btnCalculateLoad_Click();
                    }
                }).data("kendoWindow");

                //winItems.wrapper.find(".k-svg-i-download").parent().click(function (e) { 
                //    debugger;
                //    alert('winItems print click');
                //    e.preventDefault(); 
                //    tObj.add(); 
                //});
                //winItems.wrapper.find(".k-svg-i-print").parent().click(function (e) { 
                //    debugger;
                //    alert('winItems print click');
                //    e.preventDefault(); 
                //    printwinItems();  
                //});
                //$("#winItems").data("kendoWindow").wrapper.find(".k-svg-i-print").parent().click(function (e) {
                //    alert('winItems print click');
                //    e.preventDefault(); 
                //    printwinItems(); 
                //});
                //$("#winItems").data("kendoWindow").wrapper.find(".k-svg-i-download").parent().click(function (e) { 
                //    alert('winItems print click');
                //    e.preventDefault(); 
                //    CloseWinItemAndCalculate(); 
                //});
                //winAccessorials = $("#winAccessorials").kendoWindow({
                //    title: "Manage Accessorials",
                //    maxWidth: 800,
                //    modal: true,
                //    visible: false              
                //}).data("kendoWindow");

                winAddressBook = $("#winAddressBook").kendoWindow({
                    title: "Address Book",
                    maxWidth: 1000,
                    maxHeight: 550,
                    modal: true,
                    visible: false              
                }).data("kendoWindow");




                $("#txtorigCompPostalCode").kendoComboBox({
                    dataTextField: "ZipCode",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.ZipCode # #: data.City # #: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/1",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function (e) {
                        //alert("select");
                        var item = e.dataItem;
                        //console.log("item");
                        //console.log(item.City);
                        //console.log(item.State);
                        //var strText = this.value(); //strText will be populated with the value the user typed as long as the user did not hit the enter key. Enter key selection clears this value -- I haven't found a way to get it yet
                        if (item){
                            $("#txtorigCompCity").data("kendoComboBox").value(item.City);
                            $("#txtorigCompState").data("kendoComboBox").value(item.State);                                        
                        }                
                    },
                   
                });

                $("#txtdestCompPostalCode").kendoComboBox({
                    dataTextField: "ZipCode",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.ZipCode # #: data.City # #: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/1",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function(e) {
                        var item = e.dataItem;
                        console.log("Dest item");
                        console.log(item.City);
                        console.log(item.State);
                        //var strText = this.value(); //strText will be populated with the value the user typed as long as the user did not hit the enter key. Enter key selection clears this value -- I haven't found a way to get it yet
                        if (item){
                            $("#txtdestCompCity").data("kendoComboBox").value(item.City);
                            $("#txtdestCompState").data("kendoComboBox").value(item.State); 
                        } 
                    },
                });

                $("#txtorigCompCity").kendoComboBox({
                    dataTextField: "City",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.City # #: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/2",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function(e) {
                        var item = e.dataItem;                    
                        if (item){                       
                            //$("#txtorigCompPostalCode").data("kendoComboBox").value(item.ZipCode);
                            $("#txtorigCompState").data("kendoComboBox").value(item.State);   
                            $("#txtZipLocation").val("o");
                            openZipWin(item.City, item.State)
                        }                
                    },
                });

                $("#txtdestCompCity").kendoComboBox({
                    dataTextField: "City",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.City # #: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/2",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function(e) {
                        var item = e.dataItem;                    
                        if (item){                       
                            //$("#txtdestCompPostalCode").data("kendoComboBox").value(item.ZipCode);
                            $("#txtdestCompState").data("kendoComboBox").value(item.State); 
                            $("#txtZipLocation").val("d");
                            openZipWin(item.City, item.State)
                        }                
                    },
                });

                $("#txtorigCompState").kendoComboBox({
                    dataTextField: "State",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/3",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function(e) {
                        //var item = e.dataItem;                    
                        //if (item){                       
                        //    $("#txtorigCompPostalCode").data("kendoComboBox").value("");
                        //    $("#txtorigCompCity").data("kendoComboBox").value("");                        
                        //}                
                    },
                });

                $("#txtdestCompState").kendoComboBox({
                    dataTextField: "State",
                    dataValueField: "ZipCodeControl",
                    autoWidth: true,
                    template: '<span class="k-state-default"><p>#: data.State #</p></span>',                      
                    filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/ZipCode/GetZips/3",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "ZipCodeControl",
                                fields: {
                                    ZipCodeControl: { type: "number" },
                                    ZipCode: { type: "string" }, 
                                    State: { type: "string" },
                                    City: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    },
                    select: function(e) {
                        //var item = e.dataItem;                    
                        //if (item){                       
                        //    $("#txtorigCompPostalCode").data("kendoComboBox").value("");
                        //    $("#txtorigCompCity").data("kendoComboBox").value("");                        
                        //}                
                    },
                });
            
                $("#txtStopIndex").val(intStopIndex);
                $("#txtItemIndex").val(intItemIndex);        
                $("#txtItemStopIndex").val(intItemStopIndex);            
                $("#txtSelectedStopIndex").val(intSelectedStopIndex);
                $("#txtTotalStops").text(intTotalStops);
                $("#txtBookCustCompControl").val(0)
                $("#txtShipDate").kendoDatePicker({value: ngl.formatTodayForkendoDatePicker()});
                $("#txtDeliveryDate").kendoDatePicker({value: ngl.formatFutureDayForkendoDatePicker(7)});
                $("#txtWeightUnit").kendoComboBox({
                    index: 0,
                    dataTextField: "WeightUnit",
                    dataValueField: "WeightUnit",
                    dataSource: nglvLookupEditors.dsWgtUnit,
                    change: function (e) { 
                        //put change logic here 
                        return ;
                    }
                });
                $("#txtLengthUnit").kendoComboBox({
                    index: 0,
                    dataTextField: "LengthUnit",
                    dataValueField: "LengthUnit",
                    dataSource: nglvLookupEditors.dsLengthUnit,
                    change: function (e) { 
                        //put change logic here 
                        return ;
                    }
                });
                // Modified by RHR for v-8.4.0.002 on 05/07/2021 remove Readonly
                //  TotalWgt, Tota Cases and Total Plts now allow updates
                //$("#txtTotalWgt").kendoNumericTextBox({ format: "#.00 lbs", min: 1, decimals: 0, max: 9999999, spinners: false, change: totalPackageCtChanged(this) }).data("kendoNumericTextBox"); 
                $("#txtTotalWgt").kendoNumericTextBox({ format: "#.00 lbs", min: 1, decimals: 0, max: 9999999, spinners: false, change: function () {totalPackageCtChanged(this);} }).data("kendoNumericTextBox"); 
                $("#txtTotalCases").kendoNumericTextBox({format: "#0",min: 1,decimals: 0,max: 999999, spinners: false, change: function () {totalPackageCtChanged(this);} }).data("kendoNumericTextBox");                                
                $("#txtTotalPlts").kendoNumericTextBox({format: "#0",min: 1,decimals: 0,max: 99, spinners: false, change: function () {totalPackageCtChanged(this);}}).data("kendoNumericTextBox");  
                $("#txtdestStopNumber").kendoNumericTextBox({format: "#0",min: 1,decimals: 0,max: 99});
                $("#txtOrigZip").kendoMaskedTextBox();
                $("#txtDestZip").kendoMaskedTextBox();
                $("#txtSHID").kendoMaskedTextBox();
                var meSHID = $("#txtSHID").data("kendoMaskedTextBox");
                meSHID.readonly();
                $("#txtClass").kendoMaskedTextBox({ change: function () { fakClassChanged(this); } });
                $("#txtorigCompName").kendoMaskedTextBox();
                $("#txtorigCompAddress1").kendoMaskedTextBox();
                $("#txtorigCompCountry").kendoMaskedTextBox({ value: sCountry });
                $("#txtdestCompName").kendoMaskedTextBox();
                $("#txtdestCompAddress1").kendoMaskedTextBox();
                $("#txtdestCompCountry").kendoMaskedTextBox({ value: sCountry });                      
                $("#txtitemNumber").kendoMaskedTextBox();
                $("#txtFeeIndex").val(intFeeIndex);  
            
                loadOrderStopsDataSource();

                //LVV Change Accessorial
                ////var input = AccessorialsGrid.find("#comboAccessorials").kendoComboBox({
                ////    dataTextField: "Name",
                ////    dataValueField: "Code",
                ////    autoWidth: true,
                ////    template: '<span class="k-state-default"><p>#: data.Code # #: data.Name # </p></span>',
                ////    filter: "startswith",
                ////    dataSource: {
                ////        serverFiltering: true,
                ////        serverPaging: true,
                ////        transport: {
                ////            read: {
                ////                ////url: "api/Accessorials/GetAccessorialsByCarrByLE",
                ////                url: "api/Accessorials/GetAccessorialsByLegalEntityCarrier",
                ////                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                ////                type: "GET"
                ////            }
                ////        },                  
                ////        schema: { 
                ////            data: "Data",
                ////            total: "Count",
                ////            model: { 
                ////                id: "Control",
                ////                fields: {
                ////                    Control: { type: "number" },
                ////                    Code: { type: "string" }, 
                ////                    Name: { type: "string" },
                ////                    Desc: { type: "string" }
                ////                }
                ////            }, 
                ////            errors: "Errors"
                ////        },
                ////    }
                ////});
                var input = AccessorialsGrid.find("#comboAccessorials").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Description",
                    autoWidth: true,
                    //filter: "startswith",
                    dataSource: {
                        serverFiltering: true,
                        serverPaging: true,
                        transport: {
                            read: {
                                url: "api/vLookupList/GetUserDynamicList/" + nglUserDynamicLists.LEAcssCodes,
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                type: "GET"
                            }
                        },                  
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "Control",
                                fields: {
                                    Control: { type: "number" },
                                    Description: { type: "string" }, 
                                    Name: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                    }
                });//LVV Change Accessorial

                var order = new rateReqOrder();
            
                //P44RateQuotedataSource = new kendo.data.DataSource({
                //    transport: {
                //        read: {
                //            url: "api/P44RateQuote/PostRateRequest",
                //            data: {order:JSON.stringify(order)},
                //            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                //            contentType: "application/json; charset=utf-8",
                //            dataType: 'json',
                //            type: "POST",
                //            success: function(data) {
                //                options.success(data);
                //                kendo.ui.progress($(document.body), false);
                //            }
                //        },
                //        parameterMap: function (options, operation) { return options; }
                //    },
                //    schema: {
                //        data: "Data",
                //        total: "Count",
                //        model: {
                //            id: "BookControl"                        
                //        },
                //        errors: "Errors"
                //    },
                //    error: function (e) {
                //        alert(e.errors);
                //        this.cancelChanges();
                //    },
                //    serverPaging: false,
                //    serverSorting: false,
                //    serverFiltering: false
                //});

                dsAddressBook = new kendo.data.DataSource({
                    serverFiltering: true,
                    serverPaging: true,
                    pageSize: 50,
                    transport: {
                        read: {
                            url: "api/AddressBook/GetAddressBookForLE",
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            type: "GET"
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Name",
                            fields: {
                                Name: { type: "string" },
                                Address1: { type: "string" },
                                Address2: { type: "string" },
                                Address3: { type: "string" },
                                City: { type: "string" },
                                State: { type: "string" },
                                Country: { type: "string" },
                                Zip: { type: "string" }
                                //strImg: {type:"string"}
                            }
                        },
                        errors: "Errors"
                    }
                });

                dsAddressBook.read();

                $("#acFilter").kendoAutoComplete({
                    placeholder: "Search...",
                    dataTextField: "Name",
                    filter: "startswith",
                    dataSource: dsAddressBook,
                    open: function (e) {
                        //prevent drowdown list from opening
                        e.preventDefault();
                    }
                });

                $('#AddressBookGrid').kendoGrid({
                    autoBind: false,
                    dataSource: dsAddressBook,
                    selectable: "row",
                    pageable: true,
                    resizable: true,
                    groupable: false,
                    columns: [
                        //{   field: "strImg",
                        //    title: " ",
                        //    hidden: false,                       
                        //    template: function(dataItem) {
                        //        return "<span class='k-icon " + kendo.htmlEncode(dataItem.strImg) + "'></span>";
                        //    },
                        //    width: 25
                        //},
                        { field: "Name", title: "Name" },
                        { field: "Address1", title: "Address 1" },
                        { field: "Address2", title: "Address 2" },
                        { field: "Address3", title: "Address 3" },
                        { field: "City", title: "City" },
                        { field: "State", title: "State" },
                        { field: "Country", title: "Country" },
                        { field: "Zip", title: "Zip" }
                    ]
                });

                loadScreenInfo();            
            }, 10,this);  
                        
            wdgtBookPkgGridEdit = new NGLEditWindCtrl();
            wdgtBookPkgGridEdit.loadDefaults('dividEditPackagesPopupWnd', wndBookPkgGridEdit, 'id621201811071350538611498', null, objBookPkgGridDataFields, 'blueopal', 'BookPkgGridCB', null, 'NGLBookPkgGridClass', 'Packages', 'Please select a Packages record with valid data.', 'A Packages Record is Required', 'Cannot add a new Packages record because the field configuration settings are not valid, contact technical support for more information.', 'The Add New Packages Settings Are Invalid');

            wdgtAccessorialsGridEdit = new NGLEditWindCtrl();
            wdgtAccessorialsGridEdit.loadDefaults('dividAccessorialsPopupWnd', wndAccessorialsGridEdit, 'id509201911071350538611498', null, objAccessorialsGridDataFields, 'blueopal', 'AccessorialsGridCB', null, 'NGLAccessorialsGridClass', 'Accessorials', 'Please select na Accessorials record with valid data.', 'An Accessorials Record is Required', 'Cannot add a new Accessorial record because the field configuration settings are not valid, contact technical support for more information.', 'The Add New Accessorials Settings Are Invalid');
        }       
        var PageReadyJS = <%=PageReadyJS%>;                      
        menuTreeHighlightPage(); //must be called after PageReadyJS
        var divWait = $("#h1Wait");            
        if (typeof (divWait) !== 'undefined') { divWait.hide(); } 
       //Modified by RHR for v-8.5.4.001 add temperature
        $("#ddlLoadTemp").kendoDropDownList({
            dataTextField: "Description",
            dataValueField: "Name",
            autoWidth: true,
            filter: "contains",
            dataSource: {
                serverFiltering: false,
                transport: {
                    read: {
                        async: false,
                        url: "api/vLookupList/GetStaticList/" + nglStaticLists.TariffTempType,
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                    },
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
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Temp Type JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); }
            },
            ////select: function (e) {
            ////    var value = e.dataItem.Control;
            ////    if (ngl.isNullOrWhitespace(value)) { value = 0; } //Value must be int so if value is empty string or null set it to 0
            ////    pgFrtTypeFltr = value;
            ////    if (blnFirstLoad === false) { refreshPOHdrGrid(); }
            ////}
        });

   




        //var grid = $("#grid").kendoGrid({
        //    dataSource: {
        //        data: products,
        //    },
        //    scrollable: true,
        //    columns: [
        //        "ProductName",
        //        { field: "UnitPrice", title: "Unit Price", format: "{0:c}", width: "130px" },
        //        { field: "UnitsInStock", title: "Units In Stock", width: "130px" },
        //        { field: "Discontinued", width: "130px" }
        //    ]
        //}).data("kendoGrid");

    });
      
    </script>
          <style>
              .zipWin { width:98%; margin:5px; }
                      
              /* 
                .k-grid tbody .k-button  { min-width: 18px; width: 28px; }                     
                .k-grid tbody .cm-button  { color: red; min-width: 200px; width: 200px; }
              */  
                   
              #AddressBookGrid .k-grid-content { min-height: 50px; max-height: 400px; }
           
              .my-custom-icon-class:before { content: "\e13a"; } /* Adds a glyph using the Unicode character number */
          
              .form-control { font-family: append($font-family-sans-serif, "FontAwesome", "comma"); }
          
              /*            
              .k-grid-header { padding-right: 0 !important; }
              .no-scrollbar .k-grid-header { padding: 0 !important; }
              .no-scrollbar .k-grid-content { overflow-y: visible; }
              */
       </style>
          <div id="exportHistQuotes" style="display: none;">
              <div id="exportHistQuotesGrid"></div>
          </div>

          <div id="exportQuotes" style="display: none;">
              <div id="exportQuotesGrid"></div>
          </div>
      </div>
    </body>
</html>
