$(window).resize(function () {
    $("#vertical").data("kendoSplitter").trigger("resize")
});

$(document).ready(function () {

    $("#vertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: true, size: "100px" },
            { collapsible: false },
            { collapsible: false, resizable: false, size: "50px" }

        ]
    });

    $("#horizontal").kendoSplitter({
        panes: [
            { collapsible: true, size: "280px" },
            { collapsible: false }
        ]
    });
     $("#chaildvertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: false, resizable: false, size: "120px" },
            { collapsible: false },
        ]
    });
     $("#chaildhorizontal").kendoSplitter({
        panes: [
            { collapsible: false, size: "275px" },
            { collapsible: false }
        ]
    });
});
