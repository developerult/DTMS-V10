//Added By IQSS as part of EDIMaintTool/Scheduler projects

/**
 * KendoWin Config for Height , Width, Modal,visibility, action
 */
var kendoWin = {
    title: "Edit/Add",
    height: 485,
    width: 475,
    modal: true,
    visible: false,
    actions: ["save", "Minimize", "Maximize", "Close"],
};

var kendoWindow = {
    title: "Edit/Add",
    height: 485,
    width: 475,
    modal: true,
    visible: false,
    actions: ["Minimize", "Maximize", "Close"],
};
/**
 * KendoWin Config for Styling KendoWindow
 */
var kendoWinStyle = function(padding){
    $("#EditDiv").css(padding);
}
