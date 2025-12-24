function dataEntryRow() {
    caption: "";
    id: "";
    controltype: "";
    metaData: "";
}

(function () {

    // The HTML for this View
    var viewHTML;
    var viewRows;
    function refreshViewData() {

        var $html = $(viewHTML);
        // Empty Old View Contents
        var $dataContainer = $html.find(".data-container");
        $dataContainer.empty();
       
        //var $html = $("#userDataEntry");
        //var divWelcome = document.getElementById('WelcomeMessage');
        var $template = $html.find(".data-container");
        //var $template = $("#txtShipKey")
        var output = '';
        if (viewRows && viewRows.length) {
            for (var row in viewRows) {
                if (viewRows.hasOwnProperty(row)) {
                    var entryRow = viewRows[row];
                    var $entry = $template;
                    $entry.find(".data-entry-label").html(entryRow.caption);
                    output += $entry.html();
                    var rowValueHTML = '';
                    switch (entryRow.controltype) {
                        case '0': //standard html text box
                            rowValueHTML = "<input id=" + entryRow.id + " />";
                        case '13':
                            rowValueHTML = "<input id=" + entryRow.id + " />";
                        case '26':
                            rowValueHTML = "<input type='password' id=" + entryRow.id + " />";
                    }
                    $entry.find(".data-entry-value").html(rowValueHTML);
                    output += $entry.html();
                }
            }
        }        
        //$loading.hide();
        $dataContainer.html(output);
    }

    // Module
    window.dataEntryForm = {
        preProcess: function (html,dataEntryRows) {

        },
        postProcess: function (html, dataEntryRows) {
            viewHTML = html;
            viewRows = dataEntryRows;
            refreshViewData();
        },
    };

}());

