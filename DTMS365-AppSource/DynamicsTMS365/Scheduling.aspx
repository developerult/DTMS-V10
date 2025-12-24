<%@ Page Title="Scheduling Page" Language="C#"  AutoEventWireup="true" CodeBehind="Scheduling.aspx.cs" Inherits="DynamicsTMS365.Scheduling" %>

<!DOCTYPE html>

<html>
<head>
    <title>Scheduler</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
    <style>
        html,
        body {height: 100%;margin: 0;padding: 0;}

        html {font-size: 12px;font-family: Arial, Helvetica, sans-serif;overflow: hidden;}

        .hide-display {
            display: none;
        }

        .breakWord20 {
            word-break: break-all !important;
            word-wrap: break-word !important;
            vertical-align: top;
        }

        .k-grid-header .k-header {
            overflow: visible !important;
            white-space: normal !important;
        }
        #LoopDesc {
            text-overflow: ellipsis !important;
        }
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .ui-legend-container {
            color:black;font-weight:bold; 
        }
        .ui-fieldset-container {
            border-color:#BBDCEB; border-width:1px;width:1165px;margin:10px; margin-left:30px;
        }
        .ui-border-container {
            margin:20px; font-size:1.1em;
        }
        .ui-padding-container {
            padding: 10px;
        }
        .ui-th-margin {
            width: 125px;
        }
        .ui-th-margin2 {
            width: 200px;
        }
        .ui-td-margin {
            padding-left:1px;
            width: 200px;
        }
        .ui-button-margin {
            width:100px;
        }
        .ui-button-margin2 {
            width:83px;
        }
        .ui-span-container {
            font-size: small; font-weight: bold;
            color: red;position: relative;bottom: 5px;
        }
        .MTop{
            margin-top:15px;
        }
        .MBottom{
            margin-bottom:15px;
        }
        .MLeft{
            margin-left:5px;
        }
        fieldset{
             border-color:#BBDCEB;
        }
        .event-templatediv p {
            margin:5px 8px 2px;
        }
         #scheduler table,td{
             border: 0 solid rgba(0%,0%,0%,7);
        }
        .zeroColor{
            background-color:coral;
        }
        .oneColor{
            background-color:chocolate;
        }
        .twoColor{
            background-color:darkgoldenrod;
        }

       
    </style>

</head>
<body>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/menuitems.js"></script>
    <!-- added by SRP on 3/8/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" class="ui-container">
        <div id="vertical" class="ui-vertical-container">
            <%--Action Menu TabStrip--%>
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" class="ui-horizontal-container">
                    <div id="left-pane">
                        <div class="pane-content">
                            <div><span>EDI - MAINTENANCE</span></div>
                            <%--Page Navigation Menu Tree--%>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <!-- Begin Page Content -->

                        <%--Message--%>
                         <div class="MTop" ></div>

                         <%--scheduler--%>
                        <div id ="schedulerDiv" style="height:calc(100vh - 200px); overflow:hidden; margin-left:2px;">
                            <div id="scheduler" ></div>
                        </div>
                            
                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <div>
                        <span>
                            <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <% Response.WriteFile("~/Views/SchedulerAddEditWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
      
        <script type="text/javascript">
            //************* Page Variables **************************
            var PageControl = '<%=PageControl%>';   
 var tObj = this;
         var tPage = this;    
            var dsWarehouse = kendo.data.DataSource;
            var res;
            var CompControl;
            var cllAPI =false;

            $(document).ready(function () {
                var PageMenuTab = $('#tab').kendoTabStrip({
                    animation: { 
                        open: { 
                            effects: 'fadeIn'
                        } 
                    },
                    dataTextField: 'text',
                    dataContentField: 'content',
                    dataSource: [
                        { 
                            text: 'Actions',
                            content: "<input id='ddlwarehouse' style='width:300px'/>&nbsp;&nbsp;<button id='allDocks'>Display Docks<li id='submenu'></li></button>&nbsp;&nbsp;",//
                        }
                    ]
                }).data('kendoTabStrip').select(0);
            
                $("#tabstrip").kendoTabStrip({
                    animation:  {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                });

                var kendoTreeViewMenu = $('#menuTree').kendoTreeView({
                    dataUrlField: 'LinksTo',
                    dataSource: {
                        data:menuitems.data
                    },
                    loadOnDemand: false,
                   // expand:OnExpand,
                }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) { callback(e); } }; };

                control = <%=UserControl%>;

                $("#schedulerDiv").css("margin-top","1%");
              

                //*******All Action Tab Buttons*********//
               $("#allDocks").kendoButton();
                $('#allDocks')
               .mouseenter(function(){
                   $('#submenu').css('display', 'inline');
               })
               .mouseleave(function(){
                   $('#submenu').css('display', 'none');
               });
               $('#submenu').css('display', 'none');


                function startChange() {
                    var startDate = start.value(),
                    endDate = end.value();

                    if (startDate) {
                        startDate = new Date(startDate);
                        startDate.setDate(startDate.getDate());
                        end.min(startDate);
                    } else if (endDate) {
                        start.max(new Date(endDate));
                    } else {
                        endDate = new Date();
                        start.max(endDate);
                        end.min(endDate);
                    }
                }

                function endChange() {
                    var endDate = end.value(),
                    startDate = start.value();

                    if (endDate) {
                        endDate = new Date(endDate);
                        endDate.setDate(endDate.getDate());
                        start.max(endDate);
                    } else if (startDate) {
                        end.min(new Date(startDate));
                    } else {
                        endDate = new Date();
                        start.max(endDate);
                        end.min(endDate);
                    }
                }

                //*****APC call for Warehouses based on user id*****//
                $.ajax({ 
                    url: '/api/vLookupList/GetUserDynamicList/', 
                    contentType: 'application/json; charset=utf-8', 
                    dataType: 'json', 
                    async: false,
                    data: { id:nglUserDynamicLists.CompNEXTrack},
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                    success: function(data) {
                        dsWarehouse=data.Data;
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Get Company Details Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                            blnSuccess = true;
                                        }
                                        else{
                                            blnSuccess = true;
                                            strValidationMsg = "No records were found matching your search criteria";
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Get Company Details Failure"; }
                                ngl.showErrMsg("Get Company Details Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                               
                    }, 
                    error: function(result) { 
                        ngl.showErrMsg("Get Company Details Failure", result, null); 
                    } 
                }); 

                //*********kendoDropDownList for Comp/Warehouse ***********//
                $("#ddlwarehouse").kendoDropDownList({
                    dataSource:dsWarehouse,
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                });
               
                //**********on Change event for warehouse*************//
                $("#ddlwarehouse").on("change warehouse", function () {
                    CompControl = $(this).data("kendoDropDownList").dataItem().Control;
                    
                    $.ajax({ 
                        url: '/api/AMSCompDockDoor/GetRecords/', 
                        contentType: 'application/json; charset=utf-8', 
                        dataType: 'json', 
                        async: false,
                        data: { filter: JSON.stringify({"filterName":"CompDockCompControl","filterValue":CompControl,"page":1,"skip":0,"take":100}) }, 
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                        success: function(data) {
                            dsDockDoors=data.Data;
                            res=data.Data;
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                        blnErrorShown = true;
                                        ngl.showErrMsg("Get CompDockDoors Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                blnSuccess = true;
                                            }
                                            else{
                                                blnSuccess = true;
                                                strValidationMsg = "No records were found matching your search criteria";
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get CompDockDoors Failure"; }
                                    ngl.showErrMsg("Get CompDockDoors Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                               
                        }, 
                        error: function(result) { 
                            ngl.showErrMsg("Get CompDockDoors Failure", result, null); 
                        } 
                    }); 

                   $("#ddlDockDoorID").data("kendoDropDownList").setDataSource(res);

                    $('#submenu').empty();
                    for(var i =0;i<res.length;i++){
                        $('#submenu').append('<input id="'+res[i].CompDockDockDoorID+'" value="'+res[i].CompDockDockDoorID+'" style="margin-left:10px"  type="checkbox" checked><label for="'+res[i].CompDockDockDoorID+'" class="k-checkbox-label">'+res[i].CompDockDockDoorName+'</label>');
                    }

                    //scheduler.resources[0].dataSource.data(res);
                    //scheduler.view(scheduler.view().name);  
                    scheduler.dataSource.read();
                });



                var Vstart=new Date();
                var Vend = new Date();
                CompControl = $('#ddlwarehouse').data("kendoDropDownList").dataItem().Control;
              
                function scheduler_view_range(e) {
                    var view = e.sender.view();
                    Vstart = view._startDate
                    Vend = view._endDate
                    
                    if(cllAPI == true){
                        scheduler.dataSource.read();
                        cllAPI = false;
                      }
                }


                dsScheduler = new kendo.data.SchedulerDataSource({
                    transport: {
                        ServerOperation:true,
                        read: function(options) { 
                            //debugger;
                            var s = new AllFilter();
                            s.filterName = Vstart;
                            s.filterValue = Vend;

                            s.CompControlFrom =CompControl;

                            $.ajax({ 
                                url: '/api/AMSAppointment/GetRecordsTest/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                    //debugger;
                                    options.success(data.Data);
                                    //console.log(data.Data);
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        //debugger;
                                        if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) != 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get Appointment Details Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                        strValidationMsg = "No records were found matching your search criteria";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Details Failure"; }
                                            ngl.showErrMsg("Get Appointment Details Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function(result) { 
                                    options.error(result);
                                
                                } 
                            }); 
                        },
                        update: function (e) { e.success(""); },
                        create: function(e) { e.success(""); },
                        destroy: function(options) {
                            $.ajax({
                                url: 'api/AMSAppointment/DeleteAppointment', 
                                type:'Post',
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: JSON.stringify(options.data), 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Delete Appointment Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) {
                                                            ngl.showWarningMsg("Delete Appointment Failure!", "", null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appointment Failure"; }
                                            ngl.showErrMsg("Delete Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                    //refresh Scheduler
                                    if (data.Data[0]) {
                                        scheduler.dataSource.read();
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appointment Failure");
                                    ngl.showErrMsg("Delete Appointment Failure", sMsg, null); 
                                } 
                            });
                        },
                    },
                    schema: {
                        model: {
                            id: "AMSApptControl",
                            fields: {
                                AMSApptControl: { from: "AMSApptControl", type: "number" },
                                title: { from: "AMSApptCarrierName" },
                                start: { type: "date", from: "AMSApptStartDate" },
                                end: { type: "date", from: "AMSApptEndDate" },
                                description: { from: "AMSApptDescription" },
                                recurrenceId: { from: "AMSApptControl" },
                                recurrenceRule: { from: "AMSApptRecurrence" },
                                doorId: { from: "AMSApptDockdoorID",type: "number",  }
                            }
                        }
                    }
                
                
                });

                $("#scheduler").on("dblclick", 'td', function (e) {
                    if ($(e.target).hasClass("customNonwork")) {
                        ngl.showWarningMsg("User can't creat Appointment in this time period.!","");
                        scheduler.edit();
                    }
                });

                var stDate = "2018/07/17 06:00 AM";
                var endDate = "2018/07/17 06:00 PM";
                var myHours = [{ Open: false },
                     { Open: true, Start: 6, End: 18 },
                     { Open: true, Start: 6, End: 18 },
                     { Open: true, Start: 6, End: 18 }, //this End value represents 6:30pm
                     { Open: true, Start: 6, End: 18 },
                     { Open: true, Start: 6, End: 18 },
                     { Open: false }];

                //***********scheduler View***************//
                var scheduler = $("#scheduler").kendoScheduler({
                    height:"100%",
                    //date: new Date("2018/07/17"),
                    date: kendo.date.today(),
                    timezone: "Etc/UTC",
                    workDayStart: new Date(stDate),
                    workDayEnd: new Date(endDate),
                    showWorkHours: true,
                    workWeekEnd:5,
                    //allDaySlot: false,
                    //footer: false,
                    //edit: AddEditFunc,
                    //editable: false,
                    views: [
                        {
                            type: "day",
                            selected: true,
                            group: {
                                resources: ["Doors"],
                                orientation: "horizontal",
                                date: true
                            }
                        },
                        {
                            type: "week",
                            selected: false,
                            group: {
                                resources: ["Doors"],
                                orientation: "horizontal",
                                date: true
                            }
                        },
                        "month",
                        {
                            type: "timeline",
                            selected: false,
                            group: {
                                resources: ["Doors"],
                                orientation: "horizontal",
                                date: true
                            }
                        }
                    ],
                    eventTemplate:"<div class='event-templatediv'>#if (data.AMSApptLabel) {#<p>#=AMSApptLabel#</p>#}#</div>",
                    dataSource: dsScheduler ,
                    navigate: function(e) {
                        //*******Permission to Refresh the Scheduler*********//
                        cllAPI = true;
                    },
                    dataBound: function (e) {
                        var view = this.view();
                        var events = this.dataItems();
                        var eventElement;
                        var event;

                        for (var idx = 0, length = events.length; idx < length; idx++) {
                            event = events[idx];
                            //get event element
                            eventElement = view.element.find("[data-uid=" + event.uid + "]");

                            //set the backgroud of the element
                            //menu.append([{ text: "Delete", attr:{'data-id':'DeleteEvent'}, cssClass: "myClass2" }]);
                            if(event.AMSApptStatusCode== 0)
                                eventElement.addClass("zeroColor");
                            if(event.AMSApptStatusCode== 1)
                                eventElement.addClass("oneColor");
                            if(event.AMSApptStatusCode== 2)
                                eventElement.addClass("twoColor");
                            else
                                eventElement.css("background-color", "#"+event.color);
                           
                        }
                        scheduler_view_range(e);
                        BlockOffTime(this);
                    }
                    //,                   
                    //resources: [
                    //    {
                    //        field: "doorId",
                    //        name: "Doors",
                    //        dataValueField:"CompDockDockDoorID",
                    //        dataTextField:"CompDockDockDoorName",
                    //        dataSource: [],
                    //        title: "CompDockDockDoorName"
                    //    },
                    //  ]
                }).data("kendoScheduler");

                function BlockOffTime(scheduler) {
                    $(".k-scheduler-table td").each(function (i, item) {
                        var slot = scheduler.slotByElement(item);
                        if (!isBusinessHour(slot)) {
                            $(item).addClass("customNonwork");
                        }                    
                    });
                }
                //scheduler.height=550;
                function isBusinessHour(slot) {
                    var businessDay = myHours[slot.startDate.getDay()];
                    if (businessDay.Open) {
                        var slotStart = parseFloat(slot.startDate.getHours() + "." + slot.startDate.getMinutes());
                        var slotEnd = parseFloat(slot.endDate.getHours() + "." + slot.endDate.getMinutes());
                        if (slotStart >= businessDay.Start && slotEnd <= businessDay.End && slotEnd != 0) {
                            //business hour
                            return true;
                        } else {
                            //non-business hour
                            return false;
                        }
                    } else {
                        //Closed all day
                        return false;
                    }
                }

                $("#scheduler").kendoTooltip({
                    filter: ".k-event:not(.k-event-drag-hint) > div, .k-task",
                    position: "top",
                    width: 250,
                    content: function(e){
                        var target = e.target;
                        var element = target.is(".k-task") ? target : target.parent();
                        var uid = element.attr("data-uid");
                        var scheduler = target.closest("[data-role=scheduler]").data("kendoScheduler");
                        var events = scheduler.occurrenceByUid(uid);
                      
                        var content = "";
                          content = content +"<div><strong>Starts On:</strong>"+kendo.toString(events.start, 'dd/MM/yyyy HH:mm')+ "<br /><div><strong>Ends On:</strong>"+kendo.toString(events.end, 'dd/MM/yyyy HH:mm')+ "<br /><hr><div> "+ events.AMSApptHover + "'</div>'";
                        return content == "" ? "No events" : content;
                    }
                });

                //***********Deafult drop down warehouse doors loding************//
                $.ajax({ 
                    url: '/api/AMSCompDockDoor/GetRecords/', 
                    contentType: 'application/json; charset=utf-8', 
                    dataType: 'json', 
                    async: false,
                    data: { filter: JSON.stringify({"filterName":"CompDockCompControl","filterValue":CompControl,"page":1,"skip":0,"take":100}) }, 
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                    success: function(data) {
                        dsDockDoors=data.Data;
                        res=data.Data;
                    } 
                }); 

                $('#submenu').empty();
                for(var i =0;i<res.length;i++){
                    $('#submenu').append('<input id="'+res[i].CompDockDockDoorID+'" value="'+res[i].CompDockDockDoorID+'" style="margin-left:10px"   type="checkbox" checked><label for="'+res[i].CompDockDockDoorID+'" class="k-checkbox-label">'+res[i].CompDockDockDoorName+'</label>');
                }

                //scheduler.resources[0].dataSource.data(res);
                //scheduler.view(scheduler.view().name);  

                //*********checkBox on change function**********//
                $("#submenu").on("change",":checkbox",function(e) {

                    var checked = $.map($("#submenu :checked"), function(checkbox) {
                        return res.filter(function(x){ return x.CompDockDockDoorID == $(checkbox).val() })
                    });

                    if (!checked.length) {
                        $("#submenu input:first:checkbox").prop('checked', true);
                        checked = $.map($("#submenu :checked"), function(checkbox) {
                            return res.filter(function(x){ return x.CompDockDockDoorID == $(checkbox).val() })
                    });
                    }
                    scheduler.resources[0].dataSource.data(checked);
                    scheduler.view(scheduler.view().name);
               
                });

            
                //***Window DockDoor dropdown List***// 
                $("#ddlDockDoorID").kendoDropDownList({
                    dataSource: res,
                    dataTextField: "CompDockDockDoorName",
                    dataValueField: "CompDockDockDoorID",
                    autoWidth: true,
                    filter: "contains",
                });

            });
        </script>
        
         <style>
             .k-today, .k-nonwork-hour, .k-today.k-nonwork-hour {
                background-color: #FFFFFF !important;
              }
            .customNonwork, .k-today.k-nonwork-hour.customNonwork {
             background-color: #DDD !important;
              }
             .k-grid tbody .k-button {
                 min-width: 18px;
                 width: 28px;
             }
             .k-grid tbody tr td{
                vertical-align: top
             }
             .k-button{
                font-weight: bold !important;
             }
             .k-scheduler-header-wrap >table>tbody>tr:last-child{
                 background:#d9ecf5 !important;
             }
             .tblResponsive .tblResponsive-top {
                vertical-align: baseline !important;
             }
             .k-scheduler-header th {
                text-overflow: initial !important;
                box-sizing: content-box;
                vertical-align: baseline !important;
             }
       </style>
    </div>
</body>

</html>