
//function setSplitterSize(key, value) {

//}

//function getSplitterSize(key, value) {

//}

function splitter2SavePaneSettingsSuccessCallback(data) {
    splitter2PaneSettingsSaving = false;
    
    var tObj = this;
    try {      
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {  
                ngl.showErrMsg("Save Pane Settings Failure", data.Errors, tObj);
            }            
        }
    } catch (err) {
        ngl.showErrMsg(err.name, err.message, tObj);

    }
}
function splitter2SavePaneSettingsAjaxErrorCallback(xhr, textStatus, error) {
    splitter2PaneSettingsSaving = false;   
    var tObj = this;   
    ngl.showErrMsg("Save Pane Settings Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tObj);
}

var splitter2WindowResizing = false;
var splitter2PaneSettingsSaving = false;
var splitter2Loading = false;

function splitter2SavePaneSettings(e) {
    if (splitter2WindowResizing === true) { splitter2WindowResizing = false; return; }
    if (splitter2PaneSettingsSaving === true) { return; }
    if (splitter2Loading === true) { return; }
    splitter2PaneSettingsSaving = true;
    var tObj = this;
    if (typeof (control) !== 'undefined' && control !== null && control != 0) {
        setTimeout(function () {
            var setting = "[{" + kendo.format("name:'{0}',value:'{1}'", "userMenuPaneSize", getVertSize(0, "100px")) + "},{" +
                kendo.format("name:'{0}',value:'{1}'", "userMenuCollapsed", getVertCollapsed(0, false)) + "},{" +
                kendo.format("name:'{0}',value:'{1}'", "userBottomPaneSize", getVertSize(2, "35px")) + "},{" +
                kendo.format("name:'{0}',value:'{1}'", "userBottomPaneCollapsed", getVertCollapsed(2, false)) + "},{" +
                kendo.format("name:'{0}',value:'{1}'", "userLeftPaneSize", getHorzSize(0, "150px")) + "},{" +
                kendo.format("name:'{0}',value:'{1}'", "userLeftPaneCollapsed", getHorzCollapsed(0, false)) + "}]";
            var tObj = this;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            //var blnRet = oCRUDCtrl.filteredPost("cmPage/PostPaneSetting", setting, tObj, "splitter2SavePaneSettingsSuccessCallback", "splitter2SavePaneSettingsAjaxErrorCallback", true);
            blnRet = oCRUDCtrl.filteredPostJSON("cmPage/PostPaneSetting", setting, tObj, "splitter2SavePaneSettingsSuccessCallback", "splitter2SavePaneSettingsAjaxErrorCallback", true);
        }, 1, tObj);
    }

}

function setMenuPaneSize(size) {
    setVertSize(0, size);
}

function setBottomSize(size) {
    setVertSize(2, size);
}

function setVertSize(index, size) {
    try {

        var panes = $("#vertical").children(".k-pane");
        var pane = null;
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else { 
            return;
        }
        if (!pane) return;
        $("#vertical").data("kendoSplitter").size(pane, size);
    } catch (err) {

        return ;
    }
}
   

function setHorzSize(index, size) {
    try {

    
        var panes = $("#horizontal").children(".k-pane");
        var pane = null;
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else {
            return;
        }
        if (!pane) return;
        $("#horizontal").data("kendoSplitter").size(pane, size);
    } catch (err) {

        return ;
    }
}

function getVertSize(index, def) {
    try{
        var panes = $("#vertical").children(".k-pane");
        var pane = null;
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else {
            return def;
        }
        if (!pane) return def;
        return $("#vertical").data("kendoSplitter").size(pane);
    } catch (err) {

        return def;
    }
}


function getVertCollapsed(index, def) {
    try{
        var panes = $("#vertical").children(".k-pane");
        var pane = null;
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else {
            return def;
        }
        if (!pane) return def;
        if ($(pane).hasClass("k-state-collapsed")) { return true; } else { return false; }
        //return $("#vertical").data("kendoSplitter").collapsed(pane);
    } catch (err) {

        return def;
    }
   
}

function getHorzSize(index, def) {
    try{           
        var panes = $("#horizontal").children(".k-pane");
        var pane = null;
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else {
            return def;
        }
        if (!pane) return def;
        return $("#horizontal").data("kendoSplitter").size(pane);
    } catch (err) {

        return def;
    }
}


function getHorzCollapsed(index, def) {
    try {       

        var panes = $("#horizontal").children(".k-pane");
        var pane = null
        if (!isNaN(index) && index < panes.length) {
            pane = panes[index];
        } else {
            return def;
        }
        if (!pane) return def;
        if ($(pane).hasClass("k-state-collapsed")) { return true; } else { return false;}
        //return $("#horizontal").data("kendoSplitter").collapsed(pane);
    } catch (err) {

        return def;
    }
}

function expandCollapseVertSize(index, collapse) {    
    var panes = $("#vertical").children(".k-pane");
    var pane = null
    if (!isNaN(index) && index < panes.length) {
        pane = panes[index];
    } else {
        return;
    }
    if (!pane) return;
    if (collapse === false) {
        $("#vertical").data("kendoSplitter").expand(pane);
    } else {
        $("#vertical").data("kendoSplitter").collapse(pane);
    }
    
    //setVertSizeByIndex(index, size);
}


function expandCollapseHorzSize(index, collapse) {
    var panes = $("#horizontal").children(".k-pane");
    var pane = null
    if (!isNaN(index) && index < panes.length) {
        pane = panes[index];
    } else {
        return;
    }
    if (!pane) return;
    if (collapse === false) {
        $("#horizontal").data("kendoSplitter").expand(pane);
    } else {
        $("#horizontal").data("kendoSplitter").collapse(pane);
    }

}

function collapseBottomPane() {  
    expandCollapseVertSize(2, true);
}

function getUserBottomPaneSize() {
    if ( typeof (userBottomPaneSize) === 'undefined' || userBottomPaneSize === null) { return "35px"; } else { return userBottomPaneSize;}
}

function getUserBottomPaneCollapsed() {
    if (typeof (userBottomPaneCollapsed) === 'undefined' || userBottomPaneCollapsed === null) { return false; } else { if (userBottomPaneCollapsed === false) { return false; } else { return true; } }
}

function getUserMenuPaneSize() {
    if (typeof (userMenuPaneSize) === 'undefined' || userMenuPaneSize === null) { return "100px"; } else { return userMenuPaneSize; }
}

function getUserMenuPaneCollapsed() {
    if (typeof (userMenuCollapsed) === 'undefined' || userMenuCollapsed === null) { return false; } else { if (userMenuCollapsed === false) { return false; } else { return true; } }
}
//left-pane
function getUserLeftPaneSize() {
    if (typeof (userLeftPaneSize) === 'undefined' || userLeftPaneSize === null) { return "150px"; } else { return userLeftPaneSize; }
}

function getUserLeftPaneCollapsed() {
    if (typeof (userLeftPaneCollapsed) === 'undefined' || userLeftPaneCollapsed === null) { return false; } else { if (userLeftPaneCollapsed === false) { return false; } else { return true; } }
}

$(window).resize(function () {
    splitter2WindowResizing = true;
    var oVert = $("#vertical").data("kendoSplitter");
    if (typeof (oVert) !== 'undefined') {
        $("#vertical").data("kendoSplitter").trigger("resize");
    }
    
});

$(document).ready(function () {

    splitter2Loading = true;

    $("#vertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: true, size: getUserMenuPaneSize(), collapsed: getUserMenuPaneCollapsed() }, //id="menu-pane"
            { collapsible: false },
            { collapsible: true, resizable: false, size: getUserBottomPaneSize(), collapsed: getUserBottomPaneCollapsed() }  //id="bottom-pane" 
        ],
        resize: function (e) {
            splitter2SavePaneSettings(e);
        },
        collapse: function (e) {
            splitter2SavePaneSettings(e);
        },
        expand: function (e) {
            splitter2SavePaneSettings(e);
        }

    });
           
    $("#horizontal").kendoSplitter({
        panes: [
            { collapsible: true, size: getUserLeftPaneSize(), collapsed: getUserLeftPaneCollapsed() },  //id="left-pane" 
            { collapsible: false }
        ],
        resize: function (e) {
            splitter2SavePaneSettings(e);
        },
        collapse: function (e) {
            splitter2SavePaneSettings(e);
        },
        expand: function (e) {
            splitter2SavePaneSettings(e);
        }
               

    });
    splitter2Loading = false;
});

function resetPaneSettings() {
    if (typeof (control) !== 'undefined' && control !== null && control != 0) {
        setTimeout(function () {
            var setting = "[{ name: 'userMenuPaneSize', value: '150px' }, { name: 'userMenuCollapsed', value: 'false' }, { name: 'userBottomPaneSize', value: '35px' }, { name: 'userBottomPaneCollapsed', value: 'true' }, { name: 'userLeftPaneSize', value: '150px' }, { name: 'userLeftPaneCollapsed', value: 'false' }]";
            var tObj = this;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.filteredPostJSON("cmPage/PostPaneSetting", setting, tObj, "splitter2SavePaneSettingsSuccessCallback", "splitter2SavePaneSettingsAjaxErrorCallback", true);
        }, 1, tObj);
    }
}
