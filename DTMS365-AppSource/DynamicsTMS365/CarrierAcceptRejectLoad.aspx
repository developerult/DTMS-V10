<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierAcceptRejectLoad.aspx.cs" Inherits="DynamicsTMS365.CarrierAcceptRejectLoad" %>

<!DOCTYPE html>

<html>
    <head>
        <title>DTMS Carrier Accept Reject Load</title>
         <%=cssReference%>  
        <style>

    html,

    body
    {
        height:100%;
        margin:0;
        padding:0;
    }

    html
    {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

    </style>
    </head>  
    <body>
        <%=jsnosplitterScripts%> 
        <%=sWaitMessage%>      

        <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">                                                               
                <div id="page-data" class="ngl-blueBorder" > 
                <br />
                 <% Response.Write(PageErrorsOrWarnings); %>   
                    <div style="margin:5px;">
                <br />
                <!-- begin Page Content -->
                <div id="pLoadTenderMessage" class="k-block k-info-colored" style="margin:5px;">
                A load has been tendered to you.  Click&nbsp;<span class="k-icon k-i-check" ></span>&nbsp;to accept or Click&nbsp;<span class="k-icon k-i-cancel" style="color: blue;"></span>&nbsp;to reject one or more loads below
                </div>

                <div>
                    <a id="btnAccept" class="cm-icononly-button" href="#" onclick="acceptLoad(this);"><span class="k-icon k-i-check"></span>Accept</a>
                </div>
                <br />
                <div>
                    <a id="btnReject" class="cm-icononly-button" href="#" onclick="rejectLoad(this);"><span class="k-icon k-i-cancel"></span>Reject</a>
                </div>

                
               <p>If you need assistance please send an email to: <% Response.Write(TokenSupportEmail);%></p>               
               <p>Or Call: <%Response.Write(TokenSupportPhone);%></p>
                <!-- End Page Content -->
                <br />
            </div>                                                  
               
           </div>
            <div id="winRejectLoad">
              <div style="margin-left:10%; margin-right:10%">
                  <input id="txtBookControl" type="hidden" />
                  <input id="txtAcceptRejectCode" type="hidden" />
                  <input id="txtBookTrackComment" type="hidden" />
                  <input id="txtwndRLCarrierControl" type="hidden" /> <%--Added By LVV on 4/8/19 - for user associated carrier enhancement--%>
                  <input id="txtwndRLCarrierContControl" type="hidden" /> <%--Added By LVV on 4/8/19 - for user associated carrier enhancement--%>
                  <div>
                      <div style="float: left;">
                          <table class="tblResponsive">
                              <tr>
                                  <td class="tblResponsive-top">Please enter a reason for rejecting this load</td>
                              </tr>
                              <tr>
                                  <td class="tblResponsive-top"><input id="txtReason" style="width:100%;"/></td>
                              </tr>
                          </table>
                      </div>     
                  </div>  
              </div>
          </div>
            
            <% Response.Write(PageTemplates); %>
           <% Response.Write(AuthLoginNotificationHTML); %> 
            
            <script>
         <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';            
        
        var tObj = this;
        var tPage = this;                  
        var oPageObject = this;

         var tBookConsPrefix = '<%=BookConsPrefix%>';    
         var tBookControl = '<%=BookControl%>';    
         var tBookDateLoad = '<%=BookDateLoad%>';    
         var tBookDateRequired = '<%=BookDateRequired%>';    
         var tBookSHID = '<%=BookSHID%>';    
         var tCarrierControl = '<%=CarrierControl%>';    
         var tCompControl = '<%=CompControl%>';    
         var tCompName = '<%=CompName%>';    
         var tExpirationDate = '<%=ExpirationDate%>';    
         var tExpirationMinutes = '<%=ExpirationMinutes%>';    
         var tLECarAllowCarrierAcceptRejectByEmail = '<%=LECarAllowCarrierAcceptRejectByEmail%>'; 
         var tTokenSupportEmail = '<%=TokenSupportEmail%>';    
         var tTokenSupportPhone = '<%=TokenSupportPhone%>';    
         var tLaneControl = '<%=LaneControl%>';    
         var tLECarCarrierAuthCarrierAcceptRejectByEmail = '<%=LECarCarrierAuthCarrierAcceptRejectByEmail%>';    
        var tTokenSupportEmail = '<%=TokenSupportEmail%>'; 
        var tTokenSupportPhone = '<%=TokenSupportPhone%>'; 
        var tOriginNameAddressCSZ = "<%=OriginNameAddressCSZ%>"; 
        var tDestNameAddressCSZ = "<%=DestNameAddressCSZ%>"; 
        var tCarrierContControl = '<%=CarrierContControl%>'; 
        var tCarrierName = '<%=CarrierName%>'; 
                
                var winRejectLoad = kendo.ui.Window;
                $("#btnAccept").kendoButton();
                $("#btnReject").kendoButton();

                <% Response.Write(PageCustomJS); %>

        function loadAcceptedCallBack(){          
            ngl.showSuccessMsg("The selected load has been accepted!");
            if (typeof (oTenderedGrid) !== 'undefined' && ngl.isObject(oTenderedGrid)) { oTenderedGrid.dataSource.read(); } //reload the data in the grid
        }

        function loadRejectCallBack(){           
            ngl.showSuccessMsg("The selected load has been rejected!");         
            if (typeof (oTenderedGrid) !== 'undefined' && ngl.isObject(oTenderedGrid)) { oTenderedGrid.dataSource.read(); } //reload the data in the grid
        }

        function saveAjaxErrorCallback(xhr, textStatus, error){       
            var tObj = this;           
            var serrormessage = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Cannot process your request, please try again.");
            ngl.showErrMsg("Accept or Reject Error", serrormessage, tObj);
        }
       

        function acceptLoad(e){          
            if (typeof (tBookControl) !== 'undefined' && tBookControl != null && tBookControl != 0 ) {           
                var ardata = new AcceptorReject();
                ardata.BookControl = tBookControl;             
                ardata.AcceptRejectCode = 0;  //accepted
                ardata.BookTrackComment = '';
                ardata.CarrierControl = tCarrierControl; 
                ardata.CarrierContControl= 0;
                var restServiceWidget = new nglRESTCRUDCtrl();
                restServiceWidget.update("Tendered/PostSave", ardata, oPageObject, "loadAcceptedCallBack", "saveAjaxErrorCallback");
            }
        }
        
        function rejectLoad(e){           
            if (typeof (tBookControl) !== 'undefined' && tBookControl != null && tBookControl != 0 ) {
                var ardata = new AcceptorReject();
                ardata.BookControl = tBookControl;
                ardata.AcceptRejectCode = 1;  //rejected
                ardata.CarrierControl = tCarrierControl; 
                ardata.CarrierContControl= 0;
                //add the arData variables to the popup window
                $("#txtBookControl").val(tBookControl);
                $("#txtAcceptRejectCode").val(0);
                $("#txtwndRLCarrierControl").val(tCarrierControl); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                $("#txtwndRLCarrierContControl").val(0); //Added By LVV on 4/8/19 - for user associated carrier enhancement               
                winRejectLoad.center().open(); //show popup window
            }
        }
        function SaveRejectReason(){
            //Get the data from the window
            var ardata = new AcceptorReject();
            ardata.BookControl = $("#txtBookControl").val();
            ardata.AcceptRejectCode = $("#txtAcceptRejectCode").val();
            ardata.BookTrackComment = $("#txtReason").val();
            ardata.CarrierControl = $("#txtwndRLCarrierControl").val(); //Added By LVV on 4/8/19 - for user associated carrier enhancement
            ardata.CarrierContControl= $("#txtwndRLCarrierContControl").val(); //Added By LVV on 4/8/19 - for user associated carrier enhancement

            //validate that reason text is not null
            if (typeof (ardata.BookTrackComment) === 'undefined' || ardata.BookTrackComment == null) {
                ngl.showErrMsg("A Reject Reason is Required", "Enter a Reason in order to continue with the Reject", null);   
                return;
            }
            //Next validate that reason text is longer than 4 characters long
            if (ardata.BookTrackComment.length < 4) {
                ngl.showErrMsg("A Reject Reason is Required", "Enter a Reason in order to continue with the Reject", null);
                return;
            }                                  
            //move the code below to the OK buttion in popup window 
            var restServiceWidget = new nglRESTCRUDCtrl();
            restServiceWidget.update("Tendered/PostSave", ardata, oPageObject, "loadRejectCallBack", "saveAjaxErrorCallback");
            winRejectLoad.close();        
        }


        $(document).ready(function () {  
            
            var PageReadyJS = <%=PageReadyJS%>;
            if (tBookControl !=0){ 
                setTimeout(function () { 
                    //***** BEGIN winRejectLoad CODE *****

                    $("#txtReason").kendoMaskedTextBox();

                    winRejectLoad = $("#winRejectLoad").kendoWindow({
                        title: "Reject Reason",
                        width: 400,
                        height: 100,
                        minWidth: 200,
                        actions: ["save"],
                        modal: true,
                        visible: false,
                        close: function(e) {
                            //clear all the values
                            $("#txtBookControl").val(0);
                            $("#txtAcceptRejectCode").val(0);
                            $("#txtBookTrackComment").val("");
                            $("#txtwndRLCarrierControl").val(0); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                            $("#txtwndRLCarrierContControl").val(0); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                        }
                    }).data("kendoWindow");              
                    //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                    $("#winRejectLoad").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveRejectReason() });               
                    //***** END winRejectLoad CODE *****
                }, 10,this);
            }
          
         var divWait = $("#h1Wait");
         if (typeof(divWait) !== 'undefined') { divWait.hide(); }
            
         var txtMessage = kendo.format("A load with SHID number {0} and Consolidation number {1} has been tendered to &nbsp;{3}.&nbsp;<br /> This load is picking up at {4} and going to {5}.&nbsp;<br />An email was sent to you with an atached PDF file, the file contains all the details.&nbsp;<br /> Click the&nbsp;<span class='k-icon k-i-check' style='color: blue;'></span>&nbsp;button below to accept the load.  Click the &nbsp;<span class='k-icon k-i-cancel' style='color: blue;'></span>&nbsp; button below to reject the load.<br />This load may have an expiration.", tBookSHID, tBookConsPrefix, ' ', tCarrierName, tOriginNameAddressCSZ, tDestNameAddressCSZ);
         $("#pLoadTenderMessage").html(txtMessage);

         

        });


    </script>
    <style>


    </style>
        </div>
    </body>
</html>
