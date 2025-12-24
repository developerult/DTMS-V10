<!DOCTYPE html>

<html>
<head>
    <title>DTMS Template</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
    <style>
        html,
        body {
            height: 100%;
            margin: 0;
            padding: 0;
        }



        html {
            overflow: hidden;
        }
    </style>
</head>
<body>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <%--
    <script src="Scripts/kendoR32023/2014.1.318/jquery.min.js"></script>--%>
    <%--
    <script src="Scripts/kendoR32023/2014.1.318/kendo.web.min.js"></script>  --%>
    <%--
    <script src="Scripts/kendoR32023/kendo.webcomponents.min.js"></script>--%>
    <%--
    <script src="http://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>--%>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>

    <div id="example" style="height: 100%; width: 100%; margin: 5px;">
        <div id="vertical" style="height: 100%; width: 100%; ">
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%; ">
                    <div id="left-pane">
                        <div class="pane-content">
                            <h3>LTL Orders</h3>
                            <div id="LTLOrdersGrid"></div>


                        </div>
                    </div>
                    <div id="center-pane">
                        <div class="pane-content">
                            <h3>Carriers</h3>
                            <div id="carriersGrid"></div>
                        </div>
                    </div>
                    <div id="right-pane">
                        <div class="pane-content">
                            <h3>Consolidated Orders</h3>
                            <div class="demo-section k-content wide">
                                <div id="LTLlistView"></div>
                                <div id="pager" class="k-pager-wrap"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="middle-pane">
                <div class="pane-content">
                    <h3>Results</h3>
                </div>
            </div>
            <div id="bottom-pane" style="height: 100%; width: 100%; ">
                <div class="pane-content">
                    <h3>Footer</h3>
                </div>
            </div>
        </div>

        <script type="text/x-kendo-tmpl" id="template">
            <div class="product-view k-widget">
                <dl>
                    <dt>BookProNumber</dt>
                    <dd>#:BookProNumber#</dd>
                    <dt>BookConsPrefix</dt>
                    <dd>#:BookConsPrefix#</dd>
                    <dt>CompName</dt>
                    <dd>#:CompName#</dd>
                    <dt>BookDestName</dt>
                    <dd>#:BookDestName#</dd>
                </dl>
                <div class="edit-buttons">
                    <a class="k-button k-edit-button" href="\\#"><span class="k-icon k-edit"></span></a>
                    <a class="k-button k-delete-button" href="\\#"><span class="k-icon k-delete"></span></a>
                </div>
            </div>
        </script>
        <script type="text/x-kendo-tmpl" id="altTemplate">
            <div class="product-view k-widget alt">
                <span>#:BookOrigName#&nbsp;#:BookOrigCity#&nbsp;#:BookOrigState#</span>
                <span>#:BookDestName#&nbsp;#:BookDestCity#&nbsp;#:BookDestState#</span>
            </div>
        </script>

        <script type="text/x-kendo-tmpl" id="editTemplate">
            <div class="product-view k-widget">
                <dl>
                    <dt>BookProNumber</dt>
                    <dd>
                        <input type="text" class="k-textbox" data-bind="value:BookProNumber" name="BookProNumber" required="required" validationmessage="required" />
                        <span data-for="BookProNumber" class="k-invalid-msg"></span>
                    </dd>
                    <dt>BookConsPrefix</dt>
                    <dd>
                        <input type="text" class="k-textbox" data-bind="value:BookConsPrefix" name="BookConsPrefix" required="required" validationmessage="required" />
                        <span data-for="BookConsPrefix" class="k-invalid-msg"></span>
                    </dd>
                    <dt>CompName</dt>
                    <dd>
                        <input type="text" class="k-textbox" data-bind="value:CompName" name="CompName" required="required" validationmessage="required" />
                        <span data-for="CompName" class="k-invalid-msg"></span>
                    </dd>
                    <dt>BookDestName</dt>
                    <dd>
                        <input type="text" class="k-textbox" data-bind="value:BookDestName" name="BookDestName" required="required" validationmessage="required" />
                        <span data-for="BookDestName" class="k-invalid-msg"></span>
                    </dd>
                </dl>
                <div class="edit-buttons">
                    <a class="k-button k-update-button" href="\\#"><span class="k-icon k-update"></span></a>
                    <a class="k-button k-cancel-button" href="\\#"><span class="k-icon k-cancel"></span></a>
                </div>
            </div>
        </script>




        <script>
        var LTLdataSource = kendo.data.DataSource;

        $(window).resize(function () {
            $("#vertical").data("kendoSplitter").trigger("resize")
        });
        $(document).ready(function () {
            $("#vertical").kendoSplitter({
                orientation: "vertical",
                panes: [
                    { collapsible: false },
                    { collapsible: true, size: "100px" },
                    { collapsible: false, resizable: false, size: "50px" }
                ]
            });

            $("#horizontal").kendoSplitter({
                panes: [
                    { collapsible: true },
                    { collapsible: false },
                    { collapsible: true }
                ]
            });

            LTLdataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/BookLTLs",
                    update: {
                        url: function (order) {
                            return "api/BookLTLs/" + order.BookControl
                        },
                        type: "POST"
                    },
                    destroy: {
                        url: function (order) {
                            return "api/BookLTLs/" + order.BookControl
                        },
                        type: "DELETE"
                    },
                    parameterMap: function (options, operation) {

                        // if the current operation is an update
                        //if (operation === "update") {
                        //    // create a new JavaScript date object based on the current
                        //    // CarrierModDate parameter value
                        //    var d = new Date(options.CarrierModDate);
                        //    // overwrite the CarrierModDate value with a formatted value that WebAPI
                        //    // will be able to convert
                        //    options.CarrierModDate = kendo.toString(new Date(options.CarrierModDate), "MM/dd/yyyy");
                        //}

                        // ALWAYS return options
                        return options;
                    }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "BookControl",
                        fields: {
                            Control: { editable: false },
                            PRO: {
                                editable: true, nullable: false, validation: {
                                    required: {
                                        message: "Please enter a unique PRO Number to identify this order"
                                    }
                                }
                            },
                            CNS: { editable: true, nullable: false, validation: { required: true } },
                            Warehouse: { editable: true, nullable: true },
                            Destination: { editable: false }
                        }
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                pageSize: 6,
                serverPaging: true
            });


            $("#pager").kendoPager({
                dataSource: LTLdataSource
            });

            var LTLlistView = $("#LTLlistView").kendoListView({
                dataSource: LTLdataSource,
                template: kendo.template($("#template").html()),
                editTemplate: kendo.template($("#editTemplate").html()),
                altTemplate: kendo.template($("#altTemplate").html())
            }).data("kendoListView");

            //$(".k-add-button").click(function (e) {
            //    LTLlistView.add();
            //    e.preventDefault();
            //});
            var LTLOrdersGrid = $("#LTLOrdersGrid").kendoGrid({
                columns: [
                    { field: "BookControl", title: "Control", width: 75 },
                    { field: "BookProNumber", title: "PRO", width: 75 },
                    { field: "BookConsPrefix", title: "CNS", width: 75 },
                    { field: "CompName", title: "Warehouse", width: 100 },
                    { field: "BookDestName", title: "Destination", width: 100 },
                    { command: ["edit", "destroy"], title: " " }
                ],
                dataSource: LTLdataSource,
                sortable: true,
                pageable: true,
                groupable: true,
                editable: "inline",
            });




        });


        //$(function () {


        //    $("#LTLlistView").kendoListView({
        //        template: kendo.template($("#template").html()),
        //        editTemplate: kendo.template($("#editTemplate").html()),
        //        datasource: LTLdataSource,
        //        sortable: true,
        //        pageable: true,
        //        groupable: true,
        //        editable: "inline",
        //    });

        //});

        $(function () {
            $("#carriersGrid").kendoGrid({
                columns: [
                    { field: "Number", title: "No", width: 75 },
                    { field: "Name", title: "Name", width: 200 },
                    { field: "CarrierSCAC", title: "SCAC", width: 75 },
                    { field: "CarrierModUser", title: "Modified By", width: 100 },
                    { field: "CarrierModDate", title: "Modified On", format: "{0:MM/dd/yyyy}" },
                    { command: ["edit", "destroy"], title: " " }
                ],
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: "api/carriers",
                        update: {
                            url: function (carrier) {
                                return "api/carriers/" + carrier.Control
                            },
                            type: "POST"
                        },
                        destroy: {
                            url: function (carrier) {
                                return "api/carriers/" + carrier.Control
                            },
                            type: "DELETE"
                        },
                        parameterMap: function (options, operation) {

                            // if the current operation is an update
                            if (operation === "update") {
                                // create a new JavaScript date object based on the current
                                // CarrierModDate parameter value
                                var d = new Date(options.CarrierModDate);
                                // overwrite the CarrierModDate value with a formatted value that WebAPI
                                // will be able to convert
                                options.CarrierModDate = kendo.toString(new Date(options.CarrierModDate), "MM/dd/yyyy");
                            }

                            // ALWAYS return options
                            return options;
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { editable: false },
                                Number: {
                                    editable: true, nullable: false, validation: {
                                        required: {
                                            message: "Please enter a unique Number to identify this carrier"
                                        }
                                    }
                                },
                                Name: { editable: true, nullable: false, validation: { required: true} },
                                CarrierSCAC: { editable: true, nullable: true},
                                CarrierModUser: { editable: false },
                                CarrierModDate: { editable: true, nullable: true, type: "date" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) {
                        alert(e.errors);
                        this.cancelChanges();
                    },
                    pageSize: 6,
                    serverPaging: true
                }),
                sortable: true,
                pageable: true,
                groupable: true,
                editable: "inline",
            });

        });


        //$(function () {


        //});

        </script>
        <style>
            #vertical {
                height: auto;
                margin: 0 auto;
            }

            #middle-pane {
                background-color: rgba(60, 70, 80, 0.10);
            }

            #bottom-pane {
                background-color: rgba(60, 70, 80, 0.15);
            }

            #left-pane, #center-pane, #right-pane {
                background-color: rgba(60, 70, 80, 0.05);
            }

            .pane-content {
                padding: 0 10px;
            }

            .product-view {
                float: left;
                width: 50%;
                height: 300px;
                box-sizing: border-box;
                border-top: 0;
                position: relative;
            }

                .product-view:nth-child(even) {
                    border-left-width: 0;
                }

                .product-view dl {
                    margin: 10px 10px 0;
                    padding: 0;
                    overflow: hidden;
                }

                .product-view dt, dd {
                    margin: 0;
                    padding: 0;
                    width: 100%;
                    line-height: 24px;
                    font-size: 18px;
                }

                .product-view dt {
                    font-size: 11px;
                    height: 16px;
                    line-height: 16px;
                    text-transform: uppercase;
                    opacity: 0.5;
                }

                .product-view dd {
                    height: 46px;
                    overflow: hidden;
                    white-space: nowrap;
                    text-overflow: ellipsis;
                }

                    .product-view dd .k-widget,
                    .product-view dd .k-textbox {
                        font-size: 14px;
                    }

            .k-listview {
                border-width: 1px 0 0;
                padding: 0;
                overflow: hidden;
                min-height: 298px;
            }

            .edit-buttons {
                position: absolute;
                bottom: 0;
                left: 0;
                right: 0;
                text-align: right;
                padding: 5px;
                background-color: rgba(0,0,0,0.1);
            }

            .k-pager-wrap {
                border-top: 0;
            }

            span.k-invalid-msg {
                position: absolute;
                margin-left: 6px;
            }

            .k-add-button {
                margin-bottom: 2em;
            }

            .alt {
                background-color: #EEE;
            }

            @media only screen and (max-width : 620px) {

                .product-view {
                    width: 100%;
                }

                    .product-view:nth-child(even) {
                        border-left-width: 1px;
                    }
            }
        </style>
    </div>


</body>

</html>
