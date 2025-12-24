<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapRoutePlanning.aspx.cs" Inherits="DynamicsTMS365.MapRoutPlanning" %>

<!DOCTYPE html>


<html>
    <head >
        <title>DTMS Map Route Planning</title>
         <%=cssReference%>  
        <style>
            html,
            body {
                height: 100%;
                margin: 0;
                padding: 0;
            }

            html {
                font-size: 12px;
                font-family: Arial, Helvetica, sans-serif;
                overflow: hidden;
            }

            .k-grid tbody tr td {
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
            }

            .k-grid tbody tr {
                cursor: move;
            }

            .placeholder {
                outline-style: dashed;
                outline-width: 1px;
                outline-color: red;
            }

            /* Added styles for row highlighting and validation */
            .k-grid tbody tr.k-state-selected {
                background-color: lightblue !important; /* Red highlight for selected row */
            }

            .k-grid tbody tr {
                cursor: pointer; /* Change cursor to indicate clickability */
            }

            /* Validation error styles */
            .error-field {
                border: 2px solid red !important;
                background-color: #fff0f0 !important;
            }

            .required-field::after {
                content: " *";
                color: red;
                font-weight: bold;
            }
        </style>
    </head>
    <body>                   
        <%=jssplitter2Scripts%> 
        <script src="https://kendo.cdn.telerik.com/2020.3.915/js/kendo.all.min.js"></script>
        <%=sWaitMessage%>    
        <link href="https://maps-sdk.trimblemaps.com/v2/trimblemaps-2.1.2.css" rel="stylesheet">
        <script src="https://maps-sdk.trimblemaps.com/v2/trimblemaps-2.2.0.js"></script>
        <script crossorigin="anonymous" src="https://polyfill.io/v3/polyfill.min.js?features=String.prototype.startsWith%2CObject.values%2CArray.prototype.includes"></script>
        <script>
            var lat = 0;
            var lon = 0;
            //var scoord;
            var mapData = [];
            var mapsLat = [];
            var stops = [];

            //Form validation function
            function validateForm() {
                var isValid = true;

                //Clear previous error states
                $(".error-field").removeClass("error-field");

                //Validate Address
                if ($("#addr").val().trim() === "") {
                    $("#addr").addClass("error-field");
                    isValid = false;
                }

                //Validate City
                if ($("#city").val().trim() === "") {
                    $("#city").addClass("error-field");
                    isValid = false;
                }

                //Validate State
                if ($("#state").val() === "" || $("#state").val() === "Select State") {
                    $("#state").addClass("error-field");
                    isValid = false;
                }

                //Validate Zip
                var zipValue = $("#zip").val();
                var zipComboBox = $("#zip").data("kendoComboBox");
                if ((!zipValue || zipValue.trim() === "" || zipValue === "0") &&
                    (!zipComboBox || !zipComboBox.value())) {
                    $("#zip").addClass("error-field");
                    isValid = false;
                }

                return isValid;
            }


            function addStop() {
                var laddr = "";
                var lcity = "";
                var lstate = "";
                var lzip = "";
                //var scoord;

                if ($("#addr").val() != "") {
                    laddr = $("#addr").val();
                }
                if ($("#city").val() != "") {
                    lcity = $("#city").val();
                }
                if ($("#state").val() != "") {
                    lstate = $("#state").val();
                }
                if ($("#zip").val() != "" && $("#zip").val() != "0") {
                    lzip = $("#zip").val();
                }
                if (lzip == "") {
                    lzip = $("#zip").data("kendoComboBox").dataItem().ZipCode;
                }

                //userData = new StopDetails();
                //userData.Address = ngl.replaceEmptyString(laddr, '', null);
                //userData.City = lcity;
                //userData.State = lstate;
                //userData.Zip = lzip;
                //var scoord = GetgeoCoords(userData);
                TrimbleMaps.APIKey = TrimbleAPIKey // 'C36349D0A5F5D440AAC0CB8A0287F02C';
                new TrimbleMaps.Geocoder.geocode({
                    address: {
                        addr: laddr,
                        city: lcity,
                        state: lstate,
                        zip: lzip,
                        //region: TrimbleMaps.Common.Region.NA
                    },
                    listSize: 1,
                    success: function (response) {
                        //console.log(response);
                        lat = response[0]["Coords"]["Lat"];
                        lng = response[0]["Coords"]["Lon"];

                        if (lng != 0 && lat != 0) {
                            $("#addr").val('');
                            $("#city").val('');
                            $("#state").val('');
                            //$("#zip").val('');
                            $("#zip").data("kendoComboBox").value('');
                            //$('#routeStopsGrid').append("<option value='" + laddr + "," + lcity + "," + lstate + "," + lzip + "'>" + laddr + ", " + lcity + ", " + lstate + ", " + lzip + "</option>");
                            //var coord = new TrimbleMaps.LngLat(lng, lat);
                            var coord = AddMapStop(lng, lat);
                            stops.push(coord);

                            var newStopRow = { StopID: stops.length, Addr: laddr, City: lcity, State: lstate, Zip: lzip, Lon: lng, Lat: lat, Distance: 0 };

                            var grid = $("#routeStopsGrid").data("kendoGrid");
                            grid.dataSource.add(newStopRow);
                            //console.log(stops);

                            if (stops.length > 1) {
                                PopulateStopsArray();
                            }
                            refreshMapRoute(stops);
                        }
                    },
                    failure: function (response) {
                        //console.log(response);
                    }
                });
            }

            function changeMapStyle() {
                var tmapData = new TrimbleMapData();
                var stylename = $("#mapstyle").val();
                if (stylename != "") {
                    tmapData.MapStyle = stylename;
                }
                $("#myMap").empty();
                var mapstopdata = $("#routeStopsGrid").data().kendoGrid.dataSource.data();
                //Populates the address data of stops
                for (var i = 0; i < mapstopdata.length; i++) {

                    //Adds to stop data object and stops data array
                    userData = new StopDetails();
                    userData.Address = mapstopdata[i].Addr + ", " + mapstopdata[i].City + ", " + mapstopdata[i].State + " " + mapstopdata[i].Zip;
                    userData.StopDescription = "";
                    userData.StopLabel = "";
                    //userData.StopColor = getStopColor(diTrack.StopCategory, diTrack.StopCompleted);
                    userData.StopType = "TRACK";
                    userData.City = mapstopdata[i].City;
                    userData.State = mapstopdata[i].State;
                    userData.Zip = mapstopdata[i].Zip;

                    mapData.push(userData);
                }

                //Displays Trimble Maps with the stops added in page
                showTrimbleMap('myMap', tmapData, stops, null, mapData, null);
            }

            function PopulateStopsArray() {
                var vehType = $("#vehicleType").val();
                var routType = $("#routeType").val();

                var hubr = false;
                //var hubRouting = $("#hubRouting").val();
                //if(hubRouting == 0) {
                //    hubr = false;
                //} else if(hubRouting == 1) {
                //    hubr = true;
                //}

                //$.ajax(settings).done(function (response) {
                //    console.log(response);
                //})
                stops.length = 0;
                var prevLon = 0;
                var prevLat = 0;
                var Dist = 0;
                var TotalDist = 0;
                var data = $("#routeStopsGrid").data().kendoGrid.dataSource.data();
                data.forEach(function (item, index) {
                    //console.log(item);
                    //console.log(item.Lon);
                    //console.log(item.Lat);

                    if (prevLon != 0 && prevLat != 0 && item.Lon != 0 && item.Lat != 0) {
                        var settings = {
                            "async": true,
                            "crossDomain": true,
                            "url": "https://pcmiler.alk.com/apis/rest/v1.0/Service.svc/route/routeReports?stops=" + prevLon + "%2C" + prevLat + "%3B" + item.Lon + "%2C" + item.Lat + "&reports=CalcMiles&authToken=C36349D0A5F5D440AAC0CB8A0287F02C&vehType=" + vehType + "&routeType=" + routType + "&hubRouting=" + hubr,
                            "method": "GET",
                            "headers": {
                                "cache-control": "no-cache",
                                "postman-token": "4d5dbf5f-ad23-b5bc-fd43-3c5524113ae1"
                            }
                        }
                        //console.log("https://pcmiler.alk.com/apis/rest/v1.0/Service.svc/route/routeReports?stops=" + prevLon + "%2C" + prevLat + "%3B" + item.Lon + "%2C" + item.Lat + "&reports=CalcMiles&authToken=C36349D0A5F5D440AAC0CB8A0287F02C&vehType=" + vtype + "&routeType=" + rType + "&hubRouting=" + hubr);
                        prevLon = item.Lon;
                        prevLat = item.Lat;

                        $.ajax(settings).done(function (response) {
                            //console.log(response);
                            //console.log(response[0]["TMiles"]);
                            Dist = response[0]["TMiles"];
                            //console.log(Dist);
                            //TotalDist = parseFloat(TotalDist) + parseFloat(Dist);
                            //console.log(TotalDist);
                            TotalDist = (parseFloat(TotalDist) + parseFloat(Dist)).toFixed(2);
                            //console.log(Dist);
                            //console.log(TotalDist);
                            $("span.totDist").text(TotalDist.toString() + " Miles");
                            var routeStopsGrid = $("#routeStopsGrid").data("kendoGrid");
                            var data = routeStopsGrid.dataSource.data();
                            var res = $.grep(data, function (d) {
                                return d.Zip == item.Zip;
                            });

                            if (res.length > 0) {
                                var dataItem = routeStopsGrid.dataSource.getByUid(res[0].uid);

                                if (dataItem) {
                                    dataItem.set("Distance", Dist);
                                    //console.log(dataItem);
                                }
                            }
                        });
                    }
                    if (prevLon == 0 && prevLat == 0) {
                        prevLon = item.Lon;
                        prevLat = item.Lat;
                    }

                    //var coord = new TrimbleMaps.LngLat(item.Lon, item.Lat);
                    var coord = AddMapStop(item.Lon, item.Lat);
                    stops.push(coord);
                    //console.log(stops);
                });
                //console.log(TotalDist);
                //$("span.totDist").text(TotalDist.toString() + " Miles");
                var newdata = $("#routeStopsGrid").data().kendoGrid.dataSource.data();
                newdata.forEach(function (item, index) {
                    if (index == 0) {
                        var StopsGrid = $("#routeStopsGrid").data("kendoGrid");
                        var data = StopsGrid.dataSource.data();
                        var res = $.grep(data, function (d) {
                            return d.Zip == item.Zip;
                        });

                        if (res.length > 0) {
                            var dataItem = StopsGrid.dataSource.getByUid(res[0].uid);

                            if (dataItem) {
                                dataItem.set("Distance", 0);
                            }
                        }
                    }
                });
                //console.log(stops);
                //if(stops.length > 0)
                //    refreshMapRoute(stops);
            }

            function refreshMapRoute(stops) {
                //debugger;
                var reportType = [];
                var tmapData = new TrimbleMapData();
                var stylename = $("#mapstyle").val();
                if (stylename != "") {
                    tmapData.MapStyle = stylename;
                }

                //Gets Vehicle type chosen from drop down and adds to report type
                var vehType = $("#vehicleType").val();
                if (vehType != "") {
                    tmapData.VehicleType = vehType;
                }

                //Gets Route type chosen from drop down and adds to report type
                var routType = $("#routeType").val();
                if (routType != "") {
                    tmapData.RouteType = routType;
                }

                var mapstopdata = $("#routeStopsGrid").data().kendoGrid.dataSource.data();
                for (var i = 0; i < mapstopdata.length; i++) {

                    //Adds to stop data object and stops data array
                    userData = new StopDetails();
                    userData.Address = mapstopdata[i].Addr + ", " + mapstopdata[i].City + ", " + mapstopdata[i].State + " " + mapstopdata[i].Zip;
                    userData.StopDescription = "";
                    userData.StopLabel = "";
                    //userData.StopColor = getStopColor(diTrack.StopCategory, diTrack.StopCompleted);
                    userData.StopType = "TRACK";
                    userData.City = mapstopdata[i].City;
                    userData.State = mapstopdata[i].State;
                    userData.Zip = mapstopdata[i].Zip;

                    mapData.push(userData);
                }
                //debugger;
                showTrimbleMap('myMap', tmapData, stops, null, mapData, null);
                /*
                const map = new TrimbleMaps.Map({
                    container: 'myMap',
                    style: TrimbleMaps.Common.Style.TRANSPORTATION,
                    center: new TrimbleMaps.LngLat(stops[0]["lng"], stops[0]["lat"]),
                    zoom: 5
                });
                
                //report type settings is added to route
                const myRoute = new TrimbleMaps.Route({
                    routeId: "myRoute",
                    stops: stops,
                    reportType: reportType
                });
                
                map.addControl(new TrimbleMaps.NavigationControl());

                const scale = new TrimbleMaps.ScaleControl({
                    maxWidth: 80,
                    unit: 'imperial'
                });
                map.addControl(scale);
                var markerHeight = 50, markerRadius = 10, linearOffset = 25;
                var popupOffsets = {
                    'top': [0, 0],
                    'top-left': [0, 0],
                    'top-right': [0, 0],
                    'bottom': [0, -markerHeight],
                    'bottom-left': [linearOffset, (markerHeight - markerRadius + linearOffset) * -1],
                    'bottom-right': [-linearOffset, (markerHeight - markerRadius + linearOffset) * -1],
                    'left': [markerRadius, (markerHeight - markerRadius) * -1],
                    'right': [-markerRadius, (markerHeight - markerRadius) * -1]
                };

                const gfeatures = [];
                const gsourcefeature = [];

                var newdata = $("#routeStopsGrid").data().kendoGrid.dataSource.data();
                //newdata.forEach(function (item, index) {
                //    if(index == 0) {
                //        var StopsGrid = $("#routeStopsGrid").data("kendoGrid");
                //        var data = StopsGrid.dataSource.data();
                //        var res = $.grep(data, function (d) {
                //            return d.Zip == item.Zip;
                //        });
                        
                //        if(res.length > 0){                
                //            var dataItem =  StopsGrid.dataSource.getByUid(res[0].uid);
                                
                //            if(dataItem){
                //                dataItem.set("Distance",0);
                //            }
                //        }
                //    }
                //});

                for (var i = 0; i < stops.length; i++) {
                    if (i == 0) {
                        gfeatures.push({
                            type: 'Feature',
                            properties: {
                                name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
                                'description': "<font face='helvetica' color='#000' size='2'><b>" + newdata[i].Addr + ", " + newdata[i].City + ", " + newdata[i].State + " " + newdata[i].Zip + "</b></font>",
                                id: ""
                            },
                            geometry: {
                                type: 'Point',
                                coordinates: [stops[i]["lng"], stops[i]["lat"]]
                            }
                        });
                        gsourcefeature.push({
                            type: 'Feature',
                            properties: {
                                name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
                                'description': "<font face='helvetica' color='#000' size='2'><b>" + newdata[i].Addr + ", " + newdata[i].City + ", " + newdata[i].State + " " + newdata[i].Zip + "</b></font>",
                                id: ""
                            },
                            geometry: {
                                type: 'Point',
                                coordinates: [stops[i]["lng"], stops[i]["lat"]]
                            }
                        });
                    }
                    else
                    {
                        gfeatures.push({
                            type: 'Feature',
                            properties: {
                                name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
                                'description': "<font face='helvetica' color='#000' size='2'><b>" + newdata[i].Addr + ", " + newdata[i].City + ", " + newdata[i].State + " " + newdata[i].Zip + "</b></font>",
                                id: i
                            },
                            geometry: {
                                type: 'Point',
                                coordinates: [stops[i]["lng"], stops[i]["lat"]]
                            }
                        });
                    }
                }
                const geoJsonData = {
                    type: 'geojson',
                    data: {
                        type: 'FeatureCollection',
                        features: gfeatures
                    }
                };
                const geoJsonSourceData = {
                    type: 'geojson',
                    data: {
                        type: 'FeatureCollection',
                        features: gsourcefeature
                    }
                };

                map.on('load', function () {
                    myRoute.addTo(map);

                    map.addSource('hqSource', geoJsonData);
                    map.addSource('hqInitSource', geoJsonSourceData);

                    map.loadImage('../Content/marker_blue.png', function (error, image) {
                        // Add image to the map
                        map.addImage('marker-icon', image);

                        // Add layer to render marker based on datasource
                        map.addLayer({
                            id: 'hqPoints',
                            source: 'hqSource',
                            type: 'circle',
                            paint: {
                                'circle-radius': 8,
                                'circle-color': '#FFF',
                                'circle-stroke-color': '#33E',
                                'circle-stroke-width': 5
                            }
                        });

                        // Show count for clustered points
                        map.addLayer({
                            id: 'hqPointNum',
                            type: 'symbol',
                            source: 'hqSource',
                            layout: {
                                'text-field': '{id}',
                                'text-font': ['Roboto Regular'],
                                'text-size': 11
                            },
                            paint: {
                                'text-color': '#000'
                            }
                        });

                        // Add layer to render marker based on datasource
                        map.addLayer({
                            id: 'hqInitPoints',
                            source: 'hqInitSource',
                            type: 'circle',
                            paint: {
                                'circle-radius': 8,
                                'circle-color': '#FFF',
                                'circle-stroke-color': '#209B00',
                                'circle-stroke-width': 5
                            }
                        });

                        // Listen for clicks on the hqPoints layer
                        map.on('click', 'hqPoints', function (evt) {
                            //console.log(evt);
                                        
                            const popupLocation = evt.features[0].geometry.coordinates.slice();
                            const popupContent = evt.features[0].properties.description;

                            new TrimbleMaps.Popup()
                            .setLngLat(popupLocation)
                            .setHTML(popupContent)
                            .addTo(map);
                        });

                        // Create a popup, but don't add it to the map yet.
                        var popup = new TrimbleMaps.Popup({
                            closeButton: false,
                            closeOnClick: false
                        });

                        // Change cursor when hovering over a feature on the hqPoints layer
                        map.on('mouseenter', 'hqPoints', function (e) {
                            map.getCanvas().style.cursor = 'pointer';

                            var coordinates = e.features[0].geometry.coordinates.slice();
                            var description = e.features[0].properties.description;

                            // Ensure that if the map is zoomed out such that multiple
                            // copies of the feature are visible, the popup appears
                            // over the copy being pointed to.
                            while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                                coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
                            }

                            // Populate the popup and set its coordinates
                            // based on the feature found.
                            popup.setLngLat(coordinates).setHTML(description).addTo(map);
                        });

                        // Change cursor back
                        map.on('mouseleave', 'hqPoints', function () {
                            map.getCanvas().style.cursor = '';
                            popup.remove();
                        });
                    });
                });

                map.setCenter();
                */
            }

            $(document).ready(function () {
                var routestops = [];

                var dataSource = new kendo.data.DataSource({
                    data: routestops
                });

                dsStopData = new kendo.data.DataSource({
                    serverFiltering: true,
                    serverPaging: true,
                    pageSize: 50,
                    transport: {
                        read: {
                            url: "api/AddressBook/GetStopData",
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
                                StopControl: { type: "number" },
                                StopAddress1: { type: "string" },
                                StopCity: { type: "string" },
                                StopState: { type: "string" },
                                StopZip: { type: "string" }
                                //strImg: {type:"string"}
                            }
                        },
                        errors: "Errors"
                    }
                });

                dsStopData.read();

                var routeStopsGrid = $("#routeStopsGrid").kendoGrid({
                    dataSource: dsStopData,
                    navigatable: true,
                    resizable: true,
                    pageable: false,
                    height: 300,
                    //toolbar: ["create", "save", "cancel"],
                    columns: [
                        //{ field: "StopID", title: "Stop", width: 40 },
                        { field: "Addr", title: "Address", width: 110 },
                        { field: "City", title: "City", width: 80 },
                        { field: "State", title: "State", width: 50 },
                        { field: "Zip", title: "Zip Code", width: 70 },
                        { field: "Lon", title: "Lon", width: 80, hidden: true },
                        { field: "Lat", title: "Lat", width: 80, hidden: true },
                        { field: "Distance", title: "Distance (In Miles)", width: 90 },
                        //{ command: { text: "X", click: removeRow }, title: " ", width: "20" }
                        { command: [{ name: "destroy", text: " ", width: 70 }] }
                    ],
                }).data("kendoGrid");

                routeStopsGrid.table.kendoSortable({
                    filter: ">tbody >tr",
                    hint: function (element) { // Customize the hint.
                        var table = $('<table style="width: 600px;" class="k-grid k-widget"></table>'),
                            hint;

                        table.append(element.clone()); // Append the dragged element.
                        table.css("opacity", 0.7);

                        return table; // Return the hint element.
                    },
                    cursor: "move",
                    placeholder: function (element) {
                        return $('<tr colspan="8" class="placeholder"></tr>');
                    },
                    change: function (e) {
                        var skip = routeStopsGrid.dataSource.skip();
                        if (skip == undefined) {
                            skip = 0;
                        }
                        oldIndex = e.oldIndex + skip
                        newIndex = e.newIndex + skip;
                        data = routeStopsGrid.dataSource.data();
                        dataItem = routeStopsGrid.dataSource.getByUid(e.item.data("uid"));

                        routeStopsGrid.dataSource.remove(dataItem);
                        routeStopsGrid.dataSource.insert(newIndex, dataItem);
                        PopulateStopsArray();
                    }
                });

                //DELETES SELECT ROW OR STOP FROM THE ROUTE STOPS LIST
                $(document).on("click", "#routeStopsGrid tbody tr .k-grid-delete", function (e) {
                    //routeStopsGrid.removeRow($(this).closest("tr"));
                    var item = routeStopsGrid.dataItem($(this).closest("tr"));
                    var check = confirm("Do you want to delete this stop?");
                    // + JSON.stringify(item, null, 4)
                    if (check) {
                        routeStopsGrid.removeRow($(this).closest("tr"));
                    }
                    PopulateStopsArray();
                    //refreshMapRoute(stops);
                });

                $("#zip").kendoComboBox({
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
                        var item = e.dataItem;
                        //var strText = this.value(); //strText will be populated with the value the user typed as long as the user did not hit the enter key. Enter key selection clears this value -- I haven't found a way to get it yet
                        if (item) {
                            $("#city").val(item.City);
                            $("#state").val(item.State);
                        }
                    },
                });
            });
        </script>
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div id="tab" class="menuBarTab"></div> 
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>Menu</span></div>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">
                            <% Response.Write(PageErrorsOrWarnings); %>

                            <div id="divTitleLE"></div>

                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>  
                            <font face="calibri">
                                <table width="100%">
                                    <!--<tr>
                                        <td colspan="2" align="center"><h1><b>Map Route Planning</b></h1></td>
                                    </tr>-->
                                    <tr>
                                        <td width="60%" valign="top">
                                            <table border="0" width="100%" style="border-width:0px;`">
                                                <tr>
                                                    <td align="right"><h3>Select Map Style:</h3><td>
                                                    <td>
                                                        <select id="mapstyle" onchange="changeMapStyle()" style="border-color: rgb(163, 208, 228);height:25px;font-family:Calibri;font-size:14px;width:200px;">
                                                            <option value="TRANSPORTATION">TRANSPORTATION</option>
                                                            <option value="DATALIGHT">DATALIGHT</option>
                                                            <option value="DATADARK">DATADARK</option>
                                                            <option value="BASIC">BASIC</option>
                                                            <option value="TERRAIN">TERRAIN</option>
                                                            <option value="SATELLITE">SATELLITE</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table border="0" width="100%">
                                                <tr><td width="80%"><h2><b>Add New Stop</b></h2></td>
                                                <td width="20%">
                                                    <input type="button" id="btnAddStop" value="Address Book" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" style="font-weight:bold;" onclick="btnAddressBook_Click();" />
                                                </td></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="60%" valign="top"><div id="myMap" style="height: 600px; width: 100%;"></div></td>
                                        <td width="40%" valign="top">
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%"><span><b>Address:</b></span></td>
                                                    <td width="50%"><input type="text" id="addr" class="k-input" style="font-family:Calibri;font-size:14px;width:195px;" /></td>
                                                </tr>
                                                <tr>
                                                    <td><span><b>City:</b></span></td>
                                                    <td><input type="text" id="city" class="k-input" style="font-family:Calibri;font-size:14px;width:195px;" /></td>
                                                </tr>
                                                <tr>
                                                    <td><span><b>State:</b></span></td>
                                                    <td>
                                                        <select id="state" style="border-color: rgb(163, 208, 228);height:25px;font-family:Calibri;font-size:13px;width:195px;">
                                                            <option value="">Select State</option>
                                                            <option value="AL">Alabama</option>
                                                            <option value="AK">Alaska</option>
                                                            <option value="AZ">Arizona</option>
                                                            <option value="AR">Arkansas</option>
                                                            <option value="CA">California</option>
                                                            <option value="CO">Colorado</option>
                                                            <option value="CT">Connecticut</option>
                                                            <option value="DE">Delaware</option>
                                                            <option value="DC">District Of Columbia</option>
                                                            <option value="FL">Florida</option>
                                                            <option value="GA">Georgia</option>
                                                            <option value="HI">Hawaii</option>
                                                            <option value="ID">Idaho</option>
                                                            <option value="IL">Illinois</option>
                                                            <option value="IN">Indiana</option>
                                                            <option value="IA">Iowa</option>
                                                            <option value="KS">Kansas</option>
                                                            <option value="KY">Kentucky</option>
                                                            <option value="LA">Louisiana</option>
                                                            <option value="ME">Maine</option>
                                                            <option value="MD">Maryland</option>
                                                            <option value="MA">Massachusetts</option>
                                                            <option value="MI">Michigan</option>
                                                            <option value="MN">Minnesota</option>
                                                            <option value="MS">Mississippi</option>
                                                            <option value="MO">Missouri</option>
                                                            <option value="MT">Montana</option>
                                                            <option value="NE">Nebraska</option>
                                                            <option value="NV">Nevada</option>
                                                            <option value="NH">New Hampshire</option>
                                                            <option value="NJ">New Jersey</option>
                                                            <option value="NM">New Mexico</option>
                                                            <option value="NY">New York</option>
                                                            <option value="NC">North Carolina</option>
                                                            <option value="ND">North Dakota</option>
                                                            <option value="OH">Ohio</option>
                                                            <option value="OK">Oklahoma</option>
                                                            <option value="OR">Oregon</option>
                                                            <option value="PA">Pennsylvania</option>
                                                            <option value="RI">Rhode Island</option>
                                                            <option value="SC">South Carolina</option>
                                                            <option value="SD">South Dakota</option>
                                                            <option value="TN">Tennessee</option>
                                                            <option value="TX">Texas</option>
                                                            <option value="UT">Utah</option>
                                                            <option value="VT">Vermont</option>
                                                            <option value="VA">Virginia</option>
                                                            <option value="WA">Washington</option>
                                                            <option value="WV">West Virginia</option>
                                                            <option value="WI">Wisconsin</option>
                                                            <option value="WY">Wyoming</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><span><b>Zip:</b></span></td>
                                                    <td><input type="text" id="zip" style="font-family:Calibri;font-size:14px;width:200px;" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td><input type="button" id="btnAddStop" value="Add Stop" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="addStop();" style="font-weight:bold;"" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                            <table width="100%" style="border-style:none;">
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2" width="100%"><span><h3>Other Options</h3></span></td>
                                                                <td colspan="2" align="right" style="padding-right:40px;""><input type="button" id="btnrefreshMap" value="Refresh Map Route" class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" onclick="PopulateStopsArray();refreshMapRoute(stops);" style="font-weight:bold;" /></td>
                                                            </tr>
                                                            <tr height="25">
                                                                <td width="15%"><b>Routing Type:</b></td>
                                                                <td width="20%">
                                                                    <select id="routeType" style="border-color: rgb(163, 208, 228);height:25px;font-family:Calibri;font-size:13px;">
                                                                        <option value="PRACTICAL">Practical</option>
                                                                        <option value="SHORTEST">Shortest</option>
                                                                        <option value="FASTEST">Fastest</option>
                                                                    </select>
                                                                </td>
                                                                <td width="15%"><b>Vehicle Type:</b></td>
                                                                <td width="20%">
                                                                    <select id="vehicleType" style="border-color: rgb(163, 208, 228);height:25px;font-family:Calibri;font-size:13px;">
                                                                        <option value="TRUCK">Truck</option>
                                                                        <option value="LIGHT_TRUCK">Light Truck</option>
                                                                        <option value="AUTOMOBILE">Auto</option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="50%"><h2>List of Stops</h2></td>
                                                    <td width="50%"><h3>Total Distance: <span id="totDistance" class="totDist" style="font-size:18px;"></span></h3></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <%--<select id="routeStops" multiple style="font-family:Calibri;font-size:14px;width:100%;height:170px;"></select>--%>
                                                        <br />
                                                        <div id="routeStopsGrid">  
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </font>


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

          <div id="winAddressBook">             
              <div>
                  <div style="margin-bottom:5px;">
                      <span>
                          <input id="acFilter" style="width:200px"/>
                          &nbsp;
                          <button id='btnAddressAddStop' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' onclick='btnAddressAddStop_Click();'><span class='k-icon k-i-map-marker-target'></span>Add Stop</button>
                      </span>               
                  </div>
                  <div id="AddressBookGrid"></div>
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


            var winAddressBook = kendo.ui.Window;







        <% Response.Write(PageCustomJS); %>
            //***************** Widgets ******************************

            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;

            //TrimbleMaps.APIKey = "C36349D0A5F5D440AAC0CB8A0287F02C";

            if (control != 0) {
                winAddressBook = $("#winAddressBook").kendoWindow({
                    title: "Address Book",
                    maxWidth: 1000,
                    maxHeight: 550,
                    modal: true,
                    visible: false
                }).data("kendoWindow");

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
            }

            var PageReadyJS = <%=PageReadyJS%>;
            setTimeout(function () {
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            }, 10, this);
        });

            function btnAddressBook_Click() {
                var grid = $("#AddressBookGrid").data("kendoGrid");
                var autocomplete = $("#acFilter").data("kendoAutoComplete").value("");
                dsAddressBook.read();
                winAddressBook.center().open();
            }

            function btnAddressAddStop_Click() {
                var grid = $("#AddressBookGrid").data("kendoGrid");
                var item = grid.dataItem(grid.select());
                if (item != null) {
                    //console.log(item.Zip);
                    $("#addr").val(item.Address1);
                    $("#city").val(item.City);
                    $("#state").val(item.State);
                    $("#zip").data("kendoComboBox").value(item.Zip);
                    addStop();
                }
            }
        </script>  
      </div>
</body>
</html>
