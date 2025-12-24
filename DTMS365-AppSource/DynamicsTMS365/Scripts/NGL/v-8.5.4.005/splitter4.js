$(window).resize(function () {
    $("#vertical").data("kendoSplitter").trigger("resize")
});

$(document).ready(function () {
    $("#h1Wait").hide();
    $("#example").show();
    $("#vertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: false },
            { collapsible: false, resizable: false, size: "50px" }
        ]
    });

    $("#horizontal").kendoSplitter({
        panes: [
            { collapsible: false },
        ]
    });
});
