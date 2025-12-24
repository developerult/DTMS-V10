$(window).resize(function () {
    $("#vertical").data("kendoSplitter").trigger("resize")
});

$(document).ready(function () {

    $("#vertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: true, size: "40px" },
            { collapsible: false },
            { collapsible: false, resizable: false, size: "75px" }
        ]
    });

   
    $("#horizontal").kendoSplitter({
        panes: [
            { collapsible: true, size: "150px" },
            { collapsible: false },
            { collapsible: true,  size: "200px" }
        ]
    });  

});