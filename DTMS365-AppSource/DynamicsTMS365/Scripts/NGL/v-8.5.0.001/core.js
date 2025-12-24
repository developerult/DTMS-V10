/* Encapsulates functions inside of the ngl object to simulate an ngl namespace
 * Minimizes naming conflicts within the javascript parser
 * To use simply call ngl.functionName */
var ngl = {
    kmask: null,
    ConfirmationDialogRet: 0, // 0 = cancel, 1 = first buttion, 2 = second button etc...
    OkCancelConfirmationDialog: null,
    AlertDialog: null,
    //begin type of wrapper methods that return false if val is null
    isObject: function (val) {
        if (val === null) { return false; }
        return ((typeof val === 'function') || (typeof val === 'object'));
    },

    isFunction: function (val) {
        if (val === null) { return false; }
        if (typeof val === 'function') { return true; } else { return false; }
    },

    isArray: function (val) {
        if (val === null) { return false; }
        if (Array.isArray(val)) { return true; } else { return false; }
    },

    //Added By LVV on 4/24/18 
    isNullOrUndefined: function (val) {
        if (typeof (val) === 'undefined' || val == null) { return true; } else { return false; }
    },

    /* Added By LVV
    NOTE: I don't know if this is the best way to do this but I put this function here so I can at least be consistent
    If val is null or undefined return true
    If val is a string do the check
    If val is not null, undefined, or a string return false */
    isNullOrWhitespace: function (val) {
        if (val === null || typeof (val) === 'undefined') { return true; }
        if (typeof val === 'string') {
            return !val || !val.trim();
        }
        else { return false; }
        //return !input || !input.trim();
    },

    stringHasValue: function (val) {
        if (typeof (val) !== 'undefined' && val != null && val.length > 0) { return true; } else { return false; }
    },

    replaceEmptyString: function (val, sDefault, sSuffix) {
        //note: sSuffix is only used whe val is not empty and sSuffix is not empty white space allowed
        if (val === null || typeof (val) === 'undefined') { return sDefault; }
        if (typeof val === 'string') {
            if ((!val || !val.trim()) == true) { return sDefault; } else {
                if (typeof (sSuffix) !== 'undefined' && sSuffix != null && (typeof sSuffix === 'string') && sSuffix.length > 0) {
                    return val + sSuffix;
                } else {
                    return val;
                }
            }
        }
        else { return sDefault; }
    },

    /* Summary - Does the same thing as js startsWith() but is compatible with iE browser. Added By LVV on 11/14/19 */
    strStartsWith: function (str, searchvalue) {
        if (typeof (str) === 'undefined' || str == null) { return false; } //can't call substring() on null so return false
        if (typeof (searchvalue) === 'undefined' || searchvalue == null) { return false; } //can't get length of null so return false
        if (str.substring(0, searchvalue.length) === searchvalue) { return true; } else { return false; }
    },

    //end type of wrapper methods

    //***************** Begin tryParse Methods *****************
    intTryParse: function (sVal, intDef) {
        if (isNaN(intDef)) { intDef = 0; }
        var intRet = intDef
        if (typeof (sVal) === 'undefined' || sVal === null || sVal.length < 1) { return intDef; }
        intRet = parseInt(sVal);
        if (isNaN(intRet)) { intRet = intDef; }
        return intRet;
    },

    nbrTryParse: function (sVal, nbrDef) {
        if (isNaN(nbrDef)) { nbrDef = 0; }
        var nbrRet = nbrDef
        if (typeof (sVal) === 'undefined' || sVal === null || sVal.length < 1) { return nbrDef; }
        if (isNaN(sVal)) { nbrRet = nbrDef; } else { nbrRet = sVal; }
        return nbrRet;
    },
    //***************** End tryParse Methods *****************

    //***************** Begin Msg Processing Methods *****************

    /* tObj is an optional parameter pass null if calling this method from an object that does support the notifyOnError property */
    showErrMsg: function (sTitle, sMsg, tObj) {
        //debugger;
        var blnAllow = true;
        if (this.isObject(tObj)) { if (('notifyOnError' in tObj) && (tObj.notifyOnError !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
                notification.setOptions({ autoHideAfter: 10000 }); //set the error message to autoHide never
                notification.show({ title: sTitle, message: sMsg }, "error");
            } else { alert(sTitle + ": " + sMsg); }
        }
    },

    showError: function (err) {
        var sTitle = "Unexpected Error"
        var sMsg = "Please return to the home page"
        if (this.isObject(err)) {
            if ("name" in err) { sTitle = err.name; }
            if ("message" in err) { sMsg = err.message; }
        }
        if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
            notification.setOptions({ autoHideAfter: 10000 }); //set the error message to autoHide never
            notification.show({ title: sTitle, message: sMsg }, "error");
        } else { alert(sTitle + ": " + sMsg); }
    },

    showValidationMsg: function (sTitle, sMsg, tObj) {
        var blnAllow = true;
        if (this.isObject(tObj)) { if (('notifyOnValidationFailure' in tObj) && (tObj.notifyOnValidationFailure !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
                notification.setOptions({ autoHideAfter: 8000 }); //set the validation message to autoHide never
                notification.show({ title: sTitle, message: sMsg }, "error");
            } else { alert(sTitle + ": " + sMsg); }
        }
    },

    showSuccessMsg: function (sMsg, tObj) {
        var blnAllow = true;
        if (this.isObject(tObj)) { if (('notifyOnSuccess' in tObj) && (tObj.notifyOnSuccess !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
                notification.setOptions({ autoHideAfter: 2000 }); //set the success message to autoHide after 2 seconds
                notification.show({ message: sMsg }, "success");
            } else { alert(sMsg); }
        }
    },

    showInfoNotification: function (sTitle, sMsg, tObj) {
        var blnAllow = true;
        if (this.isObject(tObj)) { if (('notifyOnInfo' in tObj) && (tObj.notifyOnInfo !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
                notification.setOptions({ autoHideAfter: 5000 }); //set the info message to autoHide never
                notification.show({ title: sTitle, message: sMsg }, "info");
            } else { alert(sTitle + ": " + sMsg); }
        }
    },

    /* Added By LVV on 11/2/2017 for v-8.0 TMS 365 - NOTE: Currently no objects support the notifyOnWarning property */
    showWarningMsg: function (sTitle, sMsg, tObj, iHideAfter) {
        var blnAllow = true;
        //if (this.isObject(tObj)) { if (('notifyOnWarning' in tObj) && (tObj.notifyOnWarning !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (notification) !== 'undefined' && this.isObject(notification)) {
                if (iHideAfter) {
                    notification.setOptions({ autoHideAfter: iHideAfter });
                } else {
                    notification.setOptions({ autoHideAfter: 8000 });
                }
                notification.show({ title: sTitle, message: sMsg }, "warning");
            } else { alert(sTitle + ": " + sMsg); }
        }
    },

    /* Added By LVV on 9/11/18 for v-8.3 TMS365 Scheduler */
    showAlertNotification: function (sTitle, sMsg, tObj) {
        var blnAllow = true;
        if (this.isObject(tObj)) { if (('notifyOnAlert' in tObj) && (tObj.notifyOnAlert !== true)) { blnAllow = false; } }
        if (blnAllow === true) {
            if (typeof (alertNotification) !== 'undefined' && this.isObject(alertNotification)) {
                //alertNotification.setOptions({ autoHideAfter: 0 }); //set the info message to autoHide never
                alertNotification.show({ title: sTitle, message: sMsg }, "alert");
            } else { alert(sTitle + ": " + sMsg); }
        }
    },

    //***************** End Msg Processing Methods *****************

    encodeNGLReservedCharacters: function (sVal) {
        if (typeof (sVal) !== 'undefined' && typeof (sVal) === 'string' && sVal !== null) {
            sVal = sVal.replace("\\", String.fromCharCode(200));
            sVal = sVal.replace("/", String.fromCharCode(201));
        } else { sVal = ''; }
        return sVal;
    },

    UserValidated: function (sRequireAuthentication, control, oredirectUri) {
        //debugger;
        return ngl.UserValidated365(sRequireAuthentication, control, oredirectUri, 0)
        //var bReloading = false;
        //var suppressMsgs = true;
        //var bRequireAuthentication = false;
        //if (typeof (control) === 'undefined' || control === null) { control = 0;}
        //if (typeof (sRequireAuthentication) !== 'undefined' && sRequireAuthentication !== null && sRequireAuthentication.toString().toLowerCase() === "true") {
        //    suppressMsgs = false;
        //    bRequireAuthentication = true;
        //}      
        //if (typeof (validateUser) !== 'undefined' && validateUser !== null && typeof (validateUser) === 'function') {
        //    var blnReload = validateUser(suppressMsgs, bRequireAuthentication);
        //    if (blnReload == true && control == 0) {
        //        var uc = localStorage.NGLvar1452;
        //        if (bRequireAuthentication === true && (localStorage.SignedIn === 'f' || typeof (uc) === 'undefined' || uc === 0)) { return true; } else { document.location = oredirectUri + "?uc=" + uc; return true; }
        //    }
        //}               
        //return bReloading; 
    },
    UserValidated365: function (sRequireAuthentication, control, caller, serverUserControl) {
        //debugger;
        var bReloading = false;
        var suppressMsgs = true;
        var bRequireAuthentication = false;
        if (typeof (serverUserControl) === 'undefined' || serverUserControl === null) { serverUserControl = 0; }
        if (typeof (control) === 'undefined' || control === null) { control = 0; }
        if (typeof (sRequireAuthentication) !== 'undefined' && sRequireAuthentication !== null && sRequireAuthentication.toString().toLowerCase() === "true") {
            suppressMsgs = false;
            bRequireAuthentication = true;
        }
        if (typeof (validateUser) !== 'undefined' && validateUser !== null && typeof (validateUser) === 'function') {
            //the function validateUser will read the user settings, populate the local storage 
            // and define if the page should re-load -- return to the login page to save session data
            var blnReload = validateUser(suppressMsgs, bRequireAuthentication, caller);
            if (blnReload == true) {
                var uc = localStorage.NGLvar1452;
                if (control == 0 || (control != serverUserControl) || uc != control) {
                    //return to the Login.aspx validation logic.  this will update the 
                    //session data and redirect to the original caller.
                    document.location = "../Login?uc=" + uc + "&caller=" + caller;
                }
            }
        }
        return bReloading;
    },

    convertINtoLinearFeet: function (inches) {
        if (typeof (inches) !== 'undefined' && inches !== null && !isNaN(inches)) {
            return Math.round((inches / 12));
        } else { return 0; }
    },

    convertCMtoLinearFeet: function (CM) {
        if (typeof (CM) !== 'undefined' && CM !== null && !isNaN(CM) && CM > 0) {
            return convertINtoLinearFeet(convertCMtoInches(CM))
        } else { return 0; }
    },

    convertCMtoInches: function (inches) {
        if (typeof (inches) !== 'undefined' && inches !== null && !isNaN(inches)) {
            return (inches / 12);
        } else { return 0; }
    },

    convertMtoCM: function (meters) {
        if (typeof (meters) !== 'undefined' && meters !== null && !isNaN(meters) && meters > 0) {
            return (meters / 0.01);
        } else { return 0; }
    },

    convertMtoLinearFeet: function (meters) {
        if (typeof (meters) !== 'undefined' && meters !== null && !isNaN(meters) && meters > 0) {
            return convertCMtoLinearFeet(convertMtoCM(meters));
        } else { return 0; }
    },

    getPhoneNumberMask: function () {
        if (typeof (kmask) === 'undefined' || kmask === null) {
            kmask = new kendoMasks();
            kmask.loadDefaultMasks();
        }
        return kmask.getMask("phone_number");
    },

    Alert: function (sTitle, sContent, iWidth, iHeight) {
        //<div id=nglAlertDialog'></ div ><div id=nglConfirmDialog'></ div >
        if (typeof (sTitle) === 'undefined' || sTitle === null || ngl.isNullOrWhitespace(sTitle)) { sTitle = 'Alert'; }
        if (typeof (sContent) === 'undefined' || sContent === null || ngl.isNullOrWhitespace(sContent)) { sContent = 'Click OK to Continue'; }
        if (typeof (iWidth) === 'undefined' || iWidth === null || isNaN(iWidth)) { iWidth = '400'; }
        if (typeof (iHeight) === 'undefined' || iHeight === null || isNaN(iHeight)) { iHeight = '400'; }
        ngl.AlertDialog = null;
        //if (typeof (ngl.AlertDialog) === 'undefined' || ngl.AlertDialog == null) {
        ngl.AlertDialog = $('#nglAlertDialog');
        if (typeof (ngl.AlertDialog) !== 'undefined' && ngl.AlertDialog != null) {
            ngl.AlertDialog.kendoDialog({
                width: iWidth,
                maxWidth: iWidth,
                Height: iHeight,
                maxHeight: iHeight,
                title: sTitle,
                closable: false,
                buttonLayout: "normal",
                modal: true,
                content: sContent,
                actions: [
                    { text: 'Ok', action: function (e) { return true; }, primary: true }
                ],
                animation: {
                    open: { effects: "fade:in" },
                    close: { effects: "fade:out" }
                }
            });
        }
        //} else {
        //    ngl.AlertDialog.data("kendoDialog").modal = true;
        //    ngl.AlertDialog.data("kendoDialog").width = iWidth;
        //    ngl.AlertDialog.data("kendoDialog").maxWidth = iWidth;
        //    ngl.AlertDialog.data("kendoDialog").Height = iHeight;
        //    ngl.AlertDialog.data("kendoDialog").maxHeight = iHeight;
        //    ngl.AlertDialog.data("kendoDialog").title = sTitle;
        //    ngl.AlertDialog.data("kendoDialog").content = sContent
        //}
        ngl.AlertDialog.data("kendoDialog").open();
    },

    OkCancelConfirmation: function (sTitle, sContent, iWidth, iHeight, tObj, stringcallback, data) {
        //<div id=nglAlertDialog'></ div ><div id=nglConfirmDialog'></ div >
        if (typeof (sTitle) === 'undefined' || sTitle === null || ngl.isNullOrWhitespace(sTitle)) { sTitle = 'Alert'; }
        if (typeof (sContent) === 'undefined' || sContent === null || ngl.isNullOrWhitespace(sContent)) { sContent = 'Click OK to Continue'; }
        if (typeof (iWidth) === 'undefined' || iWidth === null || isNaN(iWidth)) { iWidth = '400'; }
        if (typeof (iHeight) === 'undefined' || iHeight === null || isNaN(iHeight)) { iHeight = '400'; }
        var blnReopen = false;
        if (typeof (ngl.OkCancelConfirmationDialog) !== 'undefined' || ngl.OkCancelConfirmationDialog != null) {
            $('#nglConfirmDialog').empty();
            ngl.OkCancelConfirmationDialog = null;
            blnReopen = true;
        }
        ngl.OkCancelConfirmationDialog = $('#nglConfirmDialog');
        if (typeof (ngl.OkCancelConfirmationDialog) !== 'undefined' && ngl.OkCancelConfirmationDialog != null) {
            ngl.OkCancelConfirmationDialog.kendoDialog({
                width: iWidth,
                maxWidth: iWidth,
                Height: iHeight,
                maxHeight: iHeight,
                title: sTitle,
                closable: false,
                buttonLayout: "normal",
                modal: true,
                content: sContent,
                actions: [
                    {
                        text: 'Ok',
                        action: function (e) {
                            if (typeof (tObj) !== 'undefined' && tObj != null) {
                                tObj[stringcallback](1, data)
                            } else {
                                if (typeof (stringcallback) !== 'undefined' && stringcallback != null && ngl.isFunction(window[stringcallback])) {
                                    window[stringcallback](1, data)
                                };
                            };
                            blnReopen = false;
                            ngl.ConfirmationDialogRet = 1;
                            return true;
                        },
                        primary: true
                    },
                    {
                        text: 'Cancel',
                        action: function (e) {
                            if (typeof (tObj) !== 'undefined' && tObj != null) {
                                tObj[stringcallback](0, data);
                            } else {
                                if (typeof (stringcallback) !== 'undefined' && stringcallback != null && ngl.isFunction(window[stringcallback])) {
                                    window[stringcallback](0, data)
                                };
                            };
                            blnReopen = false;
                            ngl.ConfirmationDialogRet = 0;
                            return true;
                        }
                    }
                ],
                animation: {
                    open: { effects: "fade:in" },
                    close: { effects: "fade:out" }
                },
                deactivate: function () { this.destroy(); }
            });
        }
        if (blnReopen === true) { ngl.OkCancelConfirmationDialog.data("kendoDialog").open(); }
        //dialog.data("kendoDialog").open();
        //} else {
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").modal = true;
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").width = iWidth;
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").maxWidth = iWidth;
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").Height = iHeight;
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").maxHeight = iHeight;
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").title = sTitle;            
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").content = sContent,
        //    ngl.OkCancelConfirmationDialog.data("kendoDialog").open();
        //}
    },

    getLocalTimeString: function (sVal, sDefault) {
        if (typeof (sDefault) === 'undefined' || sDefault === null || ngl.isNullOrWhitespace(sDefault)) { sDefault = '12:00'; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            var d = new Date(timestamp);
            var sHour = d.getHours().toString();
            if (sHour.length < 2) { sHour = ('0' + sHour).slice(-2); }
            var sMIn = d.getMinutes().toString();
            if (sMIn.length < 2) { sMIn = (sMIn + '00').slice(-2); }
            var sRet = sHour.concat(":", sMIn);
            return sRet; //d.toLocaleTimeString();
        } else { return sDefault; }
    },

    convertTimePickerToDateString: function (sVal, sDateString, sDefault) {
        if (typeof (sDefault) === 'undefined' || sDefault === null || ngl.isNullOrWhitespace(sDefault)) { sDefault = '1900-01-01 12:00'; }
        if (typeof (sDateString) === 'undefined' || sDateString === null || ngl.isNullOrWhitespace(sDateString)) { sDateString = '1900-01-01'; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            var d = new Date(timestamp);
            var sHour = d.getHours().toString();
            if (sHour.length < 2) { sHour = ('0' + sHour).slice(-2); }
            var sMIn = d.getMinutes().toString();
            if (sMIn.length < 2) { sMIn = (sMIn + '00').slice(-2); }
            var sRet = sDateString.concat(" ", sHour, ":", sMIn);
            return sRet;
        } else { return sDefault; }
    },

    getShortDateString: function (sVal, sDefault) {
        if (typeof (sDefault) === 'undefined' || sDefault === null) { sDefault = ''; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            var d = new Date(timestamp);
            var sRet = d.toLocaleDateString();
            //var sDay = d.getDate().toString();
            //var sMonth = (d.getMonth() + 1).toString();
            //var sYear = d.getYear().toString();
            ////var sHour = d.getHours().toString();
            ////var sMIn = d.getMinutes().toString();
            //var sRet = sDay.concat("/", sMonth, "/", sYear) // sHour.concat(":", sMIn);
            return sRet; //d.toLocaleTimeString();
        } else { return sDefault; }
    },

    convertDateForWindows: function (sVal, sDefault, seperator) {
        //Modified by RHR for v-8.2 on 09/19/2018 we used to avoid non-ascii characters in date string
        if (typeof (sDefault) === 'undefined' || sDefault === null) { sDefault = ''; }
        if (typeof (seperator) === 'undefined' || seperator === null) { seperator = '-'; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            var d = new Date(timestamp);
            var sDay = d.getDate().toString();
            var sMonth = (d.getMonth() + 1).toString();
            var sYear = d.getFullYear().toString();
            var sRet = sMonth.concat(seperator, sDay, seperator, sYear) // sHour.concat(":", sMIn);
            return sRet;
        } else { return sDefault; }
    },

    formatDate: function (sVal, sDefault, sFormat) {
        if (typeof (sDefault) === 'undefined' || sDefault === null) { sDefault = ''; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        if (typeof (sFormat) === 'undefined' || sFormat === null) { sFormat = 'd'; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            //var d = new Date(timestamp);
            var sRet = kendo.toString(new Date(timestamp), "d");

            //var sDay = d.getDate().toString();
            //var sMonth = (d.getMonth() + 1).toString();
            //var sYear = d.getYear().toString();
            ////var sHour = d.getHours().toString();
            ////var sMIn = d.getMinutes().toString();
            //var sRet = sDay.concat("/", sMonth, "/", sYear) // sHour.concat(":", sMIn);
            return sRet; //d.toLocaleTimeString();
        } else { return sDefault; }
    },

    formatDateTime: function (sVal, sDefault, sFormat) {
        if (typeof (sDefault) === 'undefined' || sDefault === null) { sDefault = ''; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        if (typeof (sFormat) === 'undefined' || sFormat === null) { sFormat = 't'; }
        var timestamp = Date.parse(sVal);
        if (isNaN(timestamp) == false) {
            //var d = new Date(timestamp);
            var sRet = kendo.toString(new Date(timestamp), sFormat);

            //var sDay = d.getDate().toString();
            //var sMonth = (d.getMonth() + 1).toString();
            //var sYear = d.getYear().toString();
            ////var sHour = d.getHours().toString();
            ////var sMIn = d.getMinutes().toString();
            //var sRet = sDay.concat("/", sMonth, "/", sYear) // sHour.concat(":", sMIn);
            return sRet; //d.toLocaleTimeString();
        } else { return sDefault; }
    },

    getDateFromTimeString: function (sTimeString, sDate, sDefaultTime) {
        var now = new Date();
        if (typeof (sDefaultTime) === 'undefined' || sDefaultTime === null || ngl.isNullOrWhitespace(sDefaultTime)) { sDefaultTime = '12:00'; }
        if (typeof (sDate) !== 'undefined' && sDate !== null) {
            var timestamp = Date.parse(sDate)
            if (isNaN(timestamp) == false) { now = new Date(timestamp); }
        };
        if (typeof (sTimeString) === 'undefined' || sTimeString === null || ngl.isNullOrWhitespace(sTimeString)) { sTimeString = sDefaultTime; };
        if (sTimeString.indexOf(":") !== -1) {
            var res = sTimeString.split(":");
            if (res.length > 1) { now.setHours(res[0], res[1]); }
        };
        return now;
    },
    formatTodayForkendoDatePicker: function () {
        var todayDt = new Date()      
        return kendo.toString(kendo.parseDate(todayDt), 'MM/dd/yyyy');
    },
    formatFutureDayForkendoDatePicker: function (addDays) {
        if (typeof (addDays) === 'undefined' || addDays === null) {
            addDays = 1;
        } else {
            addDays = ngl.intTryParse(addDays, 1);
        }
        var todayDt = new Date()
        var dtAdd = new Date()
        dtAdd.setDate(todayDt.getDate() + addDays);
        return kendo.toString(kendo.parseDate(dtAdd), 'MM/dd/yyyy');
    },

    currencyFormat: function (number) {
        if (typeof (number) === 'undefined' || number === null || ngl.isNullOrWhitespace(number.toString()) || isNaN(number)) { number = 0; }
        var ret = '$' + parseFloat(number).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        //alt
        //var ret = '$' + number.toFixed(2).replace(/(\d)(?=(\d{3})+$)/g, "$1,");
        return ret;
    },

    readDataSource: function (oContainer) {
        try {
            if (typeof (oContainer) !== 'undefined' && oContainer != null) {
                if (typeof (oContainer.dataSource) !== 'undefined' && oContainer.dataSource != null) { oContainer.dataSource.read(); }
            }
        }
        catch (err) {
            //do nothing
        }
    },

    formatPhoneNumber: function (strPhone) {
        var strRet = "";
        if (typeof (strPhone) !== 'undefined' && strPhone != null && strPhone.length > 0) {
            if (strPhone.length === 7) { strRet = value.replace(/^(\d{3})(\d{4}).*/, '$1-$2'); }
            else if (strPhone.length === 10) { strRet = strPhone.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'); }
            else { strRet = strPhone; }
        }
        return strRet;
    },

    addFinancialNumericClassToGridColumn: function (dsColumn, columnIndex, cell) {
        if (typeof (dsColumn) !== 'undefined' && dsColumn != null) {

            if (dsColumn == 0) {
                cell.addClass("green");
            } else if (dsColumn > 0) {
                cell.addClass("darkorange");
            } else {
                cell.addClass("red");
            }
        }
    }
}


var nglGroupSubTypeEnum = {
    "AutoComplete": 6,
    "Button": 17,
    "Checkbox": 54,
    "ColorPicker": 7,
    "ComboBox": 8,
    "DatePicker": 9,
    "DateTimePicker": 10,
    "Div": 35,
    "DropDownList": 11,
    "Editor": 12,
    "EditorCustomTool": 24,
    "EditorStandardTools": 25,
    "FastTabAction": 23,
    "FilterSelection": 20,
    "FloatBlockLeft": 28,
    "FloatLeftTable": 55,
    "FormFastTab": 22,
    "FullPageBorder": 27,
    "FullPageFooter": 45,
    "Grid": 1,
    "GridFastTab": 19,
    "GridToolBarTemplate": 47,
    "Header1": 31,
    "Header2": 32,
    "Header3": 33,
    "Header4": 34,
    "HTMLLineBreak": 44,
    "HTMLSpace": 43,
    "Images": 38,
    "KendoIconButton": 41,
    "KendoGridExportToolBar": 48,
    "kendoMobileSwitch": 16,
    "K-Icon": 42,
    "Label": 57,
    "Line": 30,
    "Link": 39,
    "ListView": 3,
    "MaskedTextBox": 13,
    "MultiSelect": 14,
    "NGLAddDependentCtrl": 51,
    "NGLEditOnPageCtrl": 53,
    "NGLEditWindCtrl": 50,
    "NGLErrWarnMsgLogCtrl": 64,
    "NGLPopupWindCtrl": 63,
    "NGLSummaryDataCtrl": 52,
    "NGLWorkFlowGroup": 59,
    "NGLWorkFlowOnOffSwitch": 60,
    "NGLWorkFlowOptionCtrl": 58,
    "NGLWorkFlowSectionCtrl": 62,
    "NGLWorkFlowYesNoSwitch": 61,
    "NumericTextBox": 15,
    "PageFooter": 40,
    "Paragraph": 37,
    "ScrollView": 5,
    "Span": 36,
    "SpreadSheet": 2,
    "StandardBorder": 26,
    "TabStrip": 18,
    "TableHeader": 56,
    "TimePicker": 49,
    "ToolTip": 46,
    "TreeList": 4,
    "TwoColumnData": 29
}
Object.freeze(nglGroupSubTypeEnum);

var nglStaticLists = {
    UOM: 0,
    LaneTran: 1,
    TempType: 2,
    State: 3,
    Seasonality: 4,
    CreditCardType: 5,
    PaymentForm: 6,
    Currency: 7,
    LoadType: 8,
    PalletType: 9,
    tblFormMenu: 10,
    tblReportMenu: 11,
    tblReportPar: 12,
    tblReportParType: 13,
    APAdjReason: 14,
    ComCodes: 15,
    PayCodes: 16,
    TranCodes: 17,
    LoadStatusCodes: 18,
    NegativeRevenueReason: 19,
    tblBracketType: 20,
    AccessorialVariableCodes: 21,
    tblParCategories: 22,
    TariffTempType: 23,
    TariffType: 24,
    ImportFileType: 25,
    tblRouteTypes: 26,
    tblStaticRoutes: 27,
    CapacityPreference: 28,
    ActionType: 29,
    tblAttribute: 30,
    tblAttributeType: 31,
    tblAction: 32,
    ColorCodeType: 33,
    ApptStatusColorCodeKey: 34,
    ApptTypeColorCodeKey: 35,
    tblPointType: 36,
    tblClassType: 37,
    tblClassTypeAnyAll: 38,
    tblModeType: 39,
    tblTariffType: 40,
    tblTarAgent: 41,
    tblTarRateType: 42,
    tblTarBracketType: 43,
    AccessorialFeeCalcType: 44,
    AccessorialFeeAllocationType: 45,
    AccessorialFeeType: 46,
    tblCountries: 47,
    tblEdiActions: 48,
    tblEdiElements: 49,
    tblEdiTypes: 50,
    tblHDM: 51,
    tblERPType: 52,
    tblIntegrationType: 53,
    DATEquipType: 54,
    tblFilterType: 55,
    cmGroupType: 56,
    cmGroupSubType: 57,
    cmDataType: 58,
    cmMenuType: 59,
    tblBidStatusCode: 60,
    tblLanguageCode: 61,
    tblDispatchType: 62,
    tblLoadTenderTransType: 63,
    vNGLAPIPalletTypes: 64,
    CalculationFactorType: 65, //Added By LVV on 6/11/18 for v-8.3 TMS365 Scheduler
    //'Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler - Merge EDI Tool Changes
    tblListType: 66,
    tblEDIDataType: 67,
    tblEDIFormattingFunctions: 68,
    tblEDIValidationTypes: 69,
    tblEDITransformationTypes: 70,
    SystemTimeZones: 71,
    SchedulerReasonCodes: 72,
    BookingAcssCodes: 73,
    APIFrtClasses: 74,
    APIWeightUnit: 75,
    APILengthUnit: 76,
    BookTransType: 77,
    AllCarriers: 78, //Added by LVV on 4/4/19 - I needed a list that would return all carriers in the database without apply any filters like user settings or LE
    LegalEntities: 79, //Added by LVV on 4/10/19 - I needed a list that would return all LEAdmins in the database plus "None"
    UserGroupCat: 80, //Added by LVV on 4/10/19
    LECompControls: 81, //Added by LVV on 4/12/19 - I needed a list that would return all LEAdmins in the database plus "None" and the control is LEAdminCompControl instead of LEAdminControl (some tables are stupid and use CompControl to link to LE instead of the LEAControl)
    CMVisibility: 82, //Added by LVV on 7/26/19
    APAuditFltrs: 83, //Added by LVV on 8/1/19
    FeatureType: 84, //Added by LVV on 2/20/20
    Version: 85, //Added by LVV on 2/20/20
    APReductionReasonCodes: 86, //Added By LVV on 4/6/20 for v-8.2.1.006
    NatFuelZones: 87, //ModDate  -- System -- 'Added By ManoRama On 25-AUG - 2020 for Carrier Fuel Index Maint Changes
    TransLeadTimeLocationOptions: 88, //Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit Time
    TransLeadTimeCalcTypes: 89, //Created by RHR for v-8.4.0.003 on 07/21/21 Colortech Enhancement Transit 
    //added by RHR for v-8.5 Task Manager Logic        
    TaskTypes: 90,
    TaskMinutes: 91,
    TaskHours: 92,
    TaskDays: 93,
    TaskMonths: 94,
    TaskWeekDays: 95
}

var nglUserDynamicLists = {
    Lane: 0,
    LaneActive: 1,
    LaneNonRestrictedCarriers: 2,
    LaneRestrictedCarriers: 3,
    LaneCrossDock: 4,
    LaneByWarehouse: 5,
    LaneActiveByWarehouse: 6,
    LaneCrossDockLists: 7,
    LaneCarrierTariff: 8,
    APCarrier: 9,
    APActiveCarrier: 10,
    APCarrierPaid: 11,
    APCarrierAmtPaid: 12,
    Carrier: 13,
    CarrierActive: 14,
    CarrAdHoc: 15,
    CarrierProName: 16,
    NatAcctNumber: 17,
    ARCompany: 18,
    APCompany: 19,
    Comp: 20,
    CompActive: 21,
    CompNEXTrack: 22,
    ChartOfAcounts: 23,
    TariffShipper: 24,
    CarrierQualValidated: 25,
    SingleSignOnAccountName: 26,
    SubscriptionAlerts: 27,
    tblFormList: 28,
    tblProcedureList: 29,
    tblReportList: 30,
    cmPage: 31,
    cmPageDetail: 32,
    AvailSSOAByUser: 33,
    LaneTariff: 34,
    CarrierTariffProName: 35,
    LEAcssCodes: 36,
    PackageDescription: 37, //Added by RHR for v-8.2 on 04/30/2019  User Dynamic list PackageDescription uses vLookupAcssCodeByLegalEntity to query all of the NAC codes mapped to NGL codes where the LE Admin Control exists in vAccByLegalEntityCarrier
    UserLEComps: 38, //Added By LVV on 132/13/19 - Gets a list of Companies associated with the logged in user's Legal Entity and filters using Role Center security (company restrictions)
    NGLExpenseCarrier: 39 //Added By LVV on 12/27/19 - Gets a list of all NGL "Expense Carriers" aka how Mickey And Bill pay utilities etc.
}

//Added By LVV on 6/19/18 for v-8.3 TMS365 Scheduler
var nglGlobalDynamicLists = {
    CarrierEquipCode: 0,
    CarrierEquipment: 1,
    vLookupERPSettings: 2,
    tblUserSecurity: 3,
    cmDataElement: 4,
    cmElementField: 5,
    cmPageDetailDataElement: 6,
    cmPageDetailElementField: 7,
    CalcFactorTypeUOM: 8,
    CalcFactorTypeForDock: 9,
    Accessorials: 10,
    AvailLECarrier: 11, //Added By LVV on 1/21/19 --> Returns list of Carriers that have not yet been assigned to the LECarrier table for the provided LE
    LECarrier: 12 //Added By LVV on 1/21/19 --> Returns list of Carriers that have been assigned to the LECarrier table for the provided LE
}


function expandFastTab(ExpandID, CollapseID, HeaderID, DetailID) {
    try {
        if (typeof (HeaderID) != 'undefined' && ExpandID != null) { $("#" + HeaderID).show(); }
        if (typeof (DetailID) != 'undefined' && DetailID != null) { $("#" + DetailID).show(); }
        if (typeof (ExpandID) != 'undefined' && ExpandID != null) { $("#" + ExpandID).hide(); }
        if (typeof (CollapseID) != 'undefined' && CollapseID != null) { $("#" + CollapseID).show(); }
        /*$("#" + CollapseID).closest('.fast-tab').css('background-color', 'LightGreen');*/
    }
    catch (err) { ngl.showErrMsg("Expand Error", err, null); }
}

function collapseFastTab(ExpandID, CollapseID, HeaderID, DetailID) {
    try {
        if (typeof (HeaderID) != 'undefined' && HeaderID != null) { $("#" + HeaderID).hide();  }
        if (typeof (DetailID) != 'undefined' && DetailID != null) { $("#" + DetailID).hide(); }
        if (typeof (ExpandID) != 'undefined' && ExpandID != null) { $("#" + ExpandID).show();  }
        if (typeof (CollapseID) != 'undefined' && CollapseID != null) { $("#" + CollapseID).hide(); }
       /* $("#" + ExpandID).closest('.fast-tab').css('background-color', 'LightGrey');*/
    }
    catch (err) { ngl.showErrMsg("Expand Error", err, null); }
}

function wizardData(key, max) {
    this.key = key;
    this.max = max;
    this.current = 1;
}

function moveNextFastTab(wizardData, PrevButton, NextButton) {
    var nextID = wizardData.current;
    try {
        if (wizardData.current >= wizardData.max) {
            $("#" + NextButton).hide();
            return;
        }
        nextID = wizardData.current + 1;
        $("#" + wizardData.key + wizardData.current.toString()).hide();
        $("#" + wizardData.key + nextID.toString()).show();
        if (nextID >= wizardData.max) {
            $("#" + NextButton).hide();
            $("#" + NextButton + "blank").show();
        }
        else {
            $("#" + NextButton).show();
            $("#" + NextButton + "blank").hide();
        }
        $("#" + PrevButton).show();
        $("#" + PrevButton + "blank").hide();
    }
    catch (err) {
        //do nothing
        alert(err);
    }
    wizardData.current = nextID;
}

function movePrevFastTab(wizardData, PrevButton, NextButton) {
    var nextID = wizardData.current
    try {
        if (wizardData.current <= 1) {
            $("#" + PrevButton).hide();
            return;
        }
        nextID = wizardData.current - 1
        $("#" + wizardData.key + wizardData.current.toString()).hide();
        $("#" + wizardData.key + nextID.toString()).show();
        if (nextID <= 1) {
            $("#" + PrevButton).hide();
            $("#" + PrevButton + "blank").show();
        }
        else {
            $("#" + PrevButton).show();
            $("#" + PrevButton + "blank").hide();
        }
        $("#" + NextButton).show();
        $("#" + NextButton + "blank").hide();
    }
    catch (err) {
        //do nothing
        alert(err);
    }
    wizardData.current = nextID
}

function isEmpty(str) {
    if (typeof (str) === 'undefined' || str === null) { return false; }
    var sToTest = str.toString();
    return (!sToTest || 0 === sToTest.length);
}


//this function returns a unique number based on a UTC millisecond time stamp
//typically used to generate a unique data key
//NOTE: if saving as a key must use bigint or 64bit integers
function getUTCTimeStamp() {
    return Date.now(); //Date.now() always returns the milliseconds since 1st Jan. 1970 UTC.
}

//this function returns the current local date-time to utc date-time
function getCurrentUTCDateTime() {
    return new Date(Date.now());
}

//this function returns the utc date-time from the date value provided
function getUTCDateTime(d) {
    //Note: getTime(); returns milliseconds since 1st Jan. 1970.
    var utc = d; //return d if the offset is zero
    var offset = d.getTimezoneOffset(); //get the difference in minutes from local time
    if (offset != 0) {
        utc = new Date(d.getTime() + (offset * 60000));  //60000 converts the minutes to milliseconds
    }
    return utc;
}

function getCurentFileName() {
    var pagePathName = window.location.pathname;
    return pagePathName.substring(pagePathName.lastIndexOf("/") + 1);
}

//Start Kendo Grid editor functions used to load editors into the Grid 
//function timeEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
//            .appendTo(container)
//            .kendoTimePicker({});
//}

//function dateTimeEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
//            .appendTo(container)
//            .kendoDateTimePicker({});
//}

//function dateEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
//            .appendTo(container)
//            .kendoDatePicker({});
//}

//function integerEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '"/>')
//            .appendTo(container)
//            .kendoNumericTextBox({
//                format: "#0",
//                min: 1,
//                decimals: 0,
//                max: 999999
//            });
//}

//function lbsEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '"/>')
//            .appendTo(container)
//            .kendoNumericTextBox({
//                format: "#.00 lbs",
//                min: 1,
//                decimals: 0,
//                max: 9999999
//            });
//}

//function decimalEditor(container, options) {
//    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
//            .appendTo(container)
//            .kendoNumericTextBox({});
//}

//End Kendo Grid editor functions



//Help Window Methods
function openHelpWindow() {
    if ((!document.getElementById("HelpWindow")) || (!document.getElementById("winCont")) || (!document.getElementById("EditCont"))) {
        ngl.showErrMsg("Missing an HTML element!", "Please contact NGL Technical Support", null)
        return;
    }
    getPageHelpNotes(PageControl, getPHNDisplay); //load content
}

var getPHNEdit = function (res) {
    if (ngl.isNullOrWhitespace(res.UserTitle)) { res.UserTitle = "User Notes"; }
    if (ngl.isNullOrWhitespace(res.CompTitle)) { res.CompTitle = "Legal Entity Specific Help Content"; }
    if (ngl.isNullOrWhitespace(res.DefaultTitle)) { res.DefaultTitle = "Default Help Content"; }
    if (ngl.isNullOrWhitespace(res.HelpWindowTitle)) { res.HelpWindowTitle = "Page Specific Help Content"; }

    switch (res.ALevel) {
        case 1:
            //NGL
            $('#txtE1').text(res.HelpWindowTitle);
            $('#editor1').data("kendoEditor").value(res.NotesL1);
            $("#txtE1PHCont").val(res.PHControlL1);
            $('#txtE2').text(res.CompTitle);
            $('#editor2').data("kendoEditor").value(res.NotesL2);
            $("#txtE2PHCont").val(res.PHControlL2);
            $('#txtE3').text(res.UserTitle);
            $('#editor3').data("kendoEditor").value(res.NotesL3);
            $("#txtE3PHCont").val(res.PHControlL3);
            $('#txtE4').text(res.DefaultTitle);
            $('#editor4').data("kendoEditor").value(res.NotesL4);
            $("#txtE4PHCont").val(res.PHControlL4);
            $("#E4Div").show();
            $("#E3Div").show();
            $("#E2Div").show();
            $("#E1Div").show();
            break;
        case 2:
            //Company
            $('#txtE2').text(res.CompTitle);
            $('#editor2').data("kendoEditor").value(res.NotesL2);
            $("#txtE2PHCont").val(res.PHControlL2);
            $('#txtE3').text(res.UserTitle);
            $('#editor3').data("kendoEditor").value(res.NotesL3);
            $("#txtE3PHCont").val(res.PHControlL3);
            $("#E4Div").hide();
            $("#E3Div").show();
            $("#E2Div").show();
            $("#E1Div").hide();
            break;
        case 3:
            //User
            $('#txtE3').text(res.UserTitle);
            $('#editor3').data("kendoEditor").value(res.NotesL3);
            $("#txtE3PHCont").val(res.PHControlL3);
            $("#E4Div").hide();
            $("#E3Div").show();
            $("#E2Div").hide();
            $("#E1Div").hide();
            break;
        default:
            $("#EditCont").hide();
            break;
    }
}

var getPHNDisplay = function (data) {
    //Clear values
    var strHelps = "";
    var strHelpTitle = "";
    strHelpTitle = data.HelpWindowTitle;
    //If PHControls are 0 that means no records were returned from the database for that level
    //User Notes first
    if (data.PHControlL3 != 0 && !ngl.isNullOrWhitespace(data.NotesL3)) { strHelps = ("<p></p><h1>" + data.UserTitle + "</h1>" + data.NotesL3 + "<hr>"); }
    //LE Notes second
    if (data.PHControlL2 != 0 && !ngl.isNullOrWhitespace(data.NotesL2)) { strHelps += ("<p></p><h1>" + data.CompTitle + "</h1>" + data.NotesL2 + "<hr>"); }
    //NGL Notes last
    if (data.PHControlL1 != 0 && !ngl.isNullOrWhitespace(data.NotesL1)) { strHelps += "<p></p><h1>" + data.HelpWindowTitle + "</h1>" + data.NotesL1; }
    else {
        //If there are no NGL Help Notes for the page show the default help file
        if (data.PHControlL4 != 0 && !ngl.isNullOrWhitespace(data.NotesL4)) { strHelps += "<p></p><h1>" + data.DefaultTitle + "</h1>" + data.NotesL4; }
        else { strHelpTitle = "Help"; strHelps += "<p></p><h1>General Help File Not Found</h1><p>NGL Admins: Please click Edit button to add content</p>"; }
    }
    $("#HelpWindow").data("kendoWindow").title(strHelpTitle);
    $("#winCont").html(strHelps);
    $("#EditCont").hide();
    $("#winCont").show();
    $("#HelpWindow").data("kendoWindow").center().open();
}

function getPageHelpNotes(control, resultFunc) {
    $.ajax({
        async: false,
        type: "GET",
        url: "api/PageHelp/GetPageHelpInfo",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: { id: control },
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get Page Help Info Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                resultFunc(data.Data[0]);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Read Data Failure"; }
                    ngl.showErrMsg("Get Page Help Info Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"); ngl.showErrMsg("getPageHelpNotes Error", sMsg, null); }
    });
}

function expandE4() { $("#spEditor4").show(); $("#ExpandE4").hide(); $("#CollapseE4").show(); }
function collapseE4() { $("#spEditor4").hide(); $("#ExpandE4").show(); $("#CollapseE4").hide(); }
function expandE3() { $("#spEditor3").show(); $("#ExpandE3").hide(); $("#CollapseE3").show(); }
function collapseE3() { $("#spEditor3").hide(); $("#ExpandE3").show(); $("#CollapseE3").hide(); }
function expandE2() { $("#spEditor2").show(); $("#ExpandE2").hide(); $("#CollapseE2").show(); }
function collapseE2() { $("#spEditor2").hide(); $("#ExpandE2").show(); $("#CollapseE2").hide(); }
function expandE1() { $("#spEditor1").show(); $("#ExpandE1").hide(); $("#CollapseE1").show(); }
function collapseE1() { $("#spEditor1").hide(); $("#ExpandE1").show(); $("#CollapseE1").hide(); }

function saveHelpEditorL1() {
    var h = new helpItem();
    h.PHControlL1 = $('#txtE1PHCont').val();
    h.NotesL1 = $('#editor1').data('kendoEditor').value();
    h.ALevel = 1;
    h.Page = PageControl;
    $.ajax({
        async: false,
        type: 'POST',
        url: 'api/PageHelp/PostSaveHelpInfo',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(h),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Save Help Notes Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                $('#txtE1PHCont').val(data.Data[0].Control);
                                ngl.showSuccessMsg("Save Successful!", null);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Help Notes Failure"; }
                    ngl.showErrMsg("Save Help Notes Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Help Editor 1", sMsg, null); }
    });
}

function saveHelpEditorL2() {
    var h = new helpItem();
    h.PHControlL2 = $('#txtE2PHCont').val();
    h.NotesL2 = $('#editor2').data('kendoEditor').value();
    h.ALevel = 2;
    h.Page = PageControl;
    $.ajax({
        async: false,
        type: 'POST',
        url: 'api/PageHelp/PostSaveHelpInfo',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(h),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Save Help Notes Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                $('#txtE2PHCont').val(data.Data[0].Control);
                                ngl.showSuccessMsg("Save Successful!", null);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Help Notes Failure"; }
                    ngl.showErrMsg("Save Help Notes Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Help Editor 2", sMsg, null); }
    });
}

function saveHelpEditorL3() {
    var h = new helpItem();
    h.PHControlL3 = $('#txtE3PHCont').val();
    h.NotesL3 = $('#editor3').data('kendoEditor').value();
    h.ALevel = 3;
    h.Page = PageControl;
    $.ajax({
        async: false,
        type: 'POST',
        url: 'api/PageHelp/PostSaveHelpInfo',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(h),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Save Help Notes Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                $('#txtE3PHCont').val(data.Data[0].Control);
                                ngl.showSuccessMsg("Save Successful!", null);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Help Notes Failure"; }
                    ngl.showErrMsg("Save Help Notes Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Help Editor 3", sMsg, null); }
    });
}

function saveHelpEditorL4() {
    var h = new helpItem();
    h.PHControlL4 = $('#txtE4PHCont').val();
    h.NotesL4 = $('#editor4').data('kendoEditor').value();
    h.ALevel = 4;
    h.Page = PageControl;
    $.ajax({
        async: false,
        type: 'POST',
        url: 'api/PageHelp/PostSaveHelpInfo',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(h),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Save Help Notes Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                $('#txtE4PHCont').val(data.Data[0].Control);
                                ngl.showSuccessMsg("Save Successful!", null);
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Save Help Notes Failure"; }
                    ngl.showErrMsg("Save Help Notes Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Help Editor 4", sMsg, null); }
    });
}

//Editor Content Methods
function getEditorContentNoAuth(s, resultFunc) {
    $.ajax({
        async: true,
        type: 'GET',
        url: 'api/Editor/GetEditorContentNoAuth',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: { filter: s },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get Editor Content Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                if (ngl.isFunction(resultFunc)) {
                                    resultFunc(data.Data[0]);
                                }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Editor Content not found"; }
                    ngl.showErrMsg("Get Editor Content Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg('getEditorContentNoAuth  ' + err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"); ngl.showErrMsg("Read Editor Content", sMsg, null); }
    });
}

function saveEditorContent(h, resultFunc) {
    $.ajax({
        async: false,
        type: 'POST',
        url: 'api/Editor/SaveEditor',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(h),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Save Editor Content Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                blnSuccess = true;
                                if (ngl.isFunction(resultFunc)) {
                                    resultFunc(data.Data[0]);
                                    ngl.showSuccessMsg("Save Successful!", null);
                                }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Save editor content could not be completed."; }
                    ngl.showErrMsg("Save Editor Content Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Editor Content", sMsg, null); }
    });
}

function sendEmail(emailTo, emailFrom, emailCc, emailBcc, subject, body) {
    var blnRet = 0;
    var fields = "";
    var strSp = "";
    if (ngl.isNullOrWhitespace(emailTo)) { fields += (strSp + "To"); strSp = ", "; }
    if (ngl.isNullOrWhitespace(emailFrom)) { fields += (strSp + "From"); strSp = ", "; }
    if (ngl.isNullOrWhitespace(subject)) { fields += (strSp + "Subject"); strSp = ", "; }
    if (ngl.isNullOrWhitespace(body)) { fields += (strSp + "Body"); strSp = ", "; }
    if (!ngl.isNullOrWhitespace(fields)) { ngl.showErrMsg("Required Fields", fields, null); blnRet = 0; return blnRet; }
    var em = new EmailObject();
    em.emailTo = emailTo;
    em.emailFrom = emailFrom;
    em.emailCc = emailCc;
    em.emailBcc = emailBcc;
    em.emailSubject = subject;
    em.emailBody = body;
    $.ajax({
        async: false,
        type: "POST",
        url: "api/Email/GenerateEmail",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(em),
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.StatusCode) !== 'undefined' && data.StatusCode != null && data.StatusCode === 200) { ngl.showSuccessMsg("Email Sent", null); blnRet = 1; }
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) { ngl.showErrMsg("Send Email Failure", data.Errors, null); blnRet = 0; }
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Unexpected"); ngl.showErrMsg("JSON Response Error", sMsg, null); blnRet = 0; }
    });
    return blnRet;
}

function getLEAdmin(id, resultFunc) {
    $.ajax({
        async: true,
        type: 'GET',
        url: 'api/LegalEntity/GetLEAdmin/' + id,
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get LE Admin Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                if (ngl.isFunction(resultFunc)) {
                                    resultFunc(data.Data[0]);
                                }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "LE Admin not found"; }
                    ngl.showErrMsg("Get LE Admin Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg('getLEAdmin  ' + err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"); ngl.showErrMsg("Read Editor Content", sMsg, null); }
    });
}

function getLEAdminNotAsync(id, resultFunc) {
    $.ajax({
        async: false,
        type: 'GET',
        url: 'api/LegalEntity/GetLEAdmin/' + id,
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get LE Admin Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                if (ngl.isFunction(resultFunc)) { resultFunc(data.Data); }
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "LE Admin not found"; }
                    ngl.showErrMsg("Get LE Admin Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg('getLEAdmin  ' + err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"); ngl.showErrMsg("Read Editor Content", sMsg, null); }
    });
}


function formatAjaxJSONResponsMsgs(xhr, textStatus, error, strDefault) {
    var sRetMsg = strDefault;
    if (typeof (xhr.responseJSON) != 'undefined' && xhr.responseJSON != null) {
        var xhrMessage = xhr.responseJSON.Message;
        var xhrExceptionType = xhr.responseJSON.ExceptionType;
        var xhrExceptionMessage = xhr.responseJSON.ExceptionMessage;
        sRetMsg = xhrMessage.concat(": ", (xhrExceptionMessage || ''));
        var xhrInnerException = xhr.responseJSON.InnerException;
        var xhrInnerExceptionMsg = "";
        var xhrInnerExceptionType = "";
        if (typeof (xhrInnerException) != 'undefined' && xhrInnerException != null) {
            xhrInnerExceptionMsg = xhrInnerException.ExceptionMessage;
            xhrInnerExceptionType = xhrInnerException.ExceptionType;
            //Note: the word details is not localized and may need future clarification
            //javascript message localization is not complete
            sRetMsg = sRetMsg.concat(" details: ", (xhrInnerExceptionMsg || ''));
        }
    } else if (typeof (xhr.errors) != 'undefined' && xhr.errors != null && !isEmpty(xhr.errors)) {
        sRetMsg = xhr.errors;
    } else {
        var blnShowMsg = false;
        if (typeof (xhr.responseText) != 'undefined' && xhr.responseText != null && !isEmpty(xhr.responseText)) {
            try {
                var err = eval("(" + xhr.responseText + ")");
                if (typeof (err) != 'undefined' && err != null) {
                    if (typeof (err.Message) != 'undefined' && err.Message != null) { sRetMsg = sRetMsg + err.Message; blnShowMsg = true; }
                    if (typeof (err.Errors) != 'undefined' && err.Errors != null) { sRetMsg = sRetMsg + ' ' + err.Errors; blnShowMsg = true; }
                }
            } catch (err) {
                alert("Error Details:\n" + xhr.responseText)
            }
        }
        if (blnShowMsg == false) {
            if (typeof (xhr.statusText) != 'undefined' && xhr.statusText != null) {
                if (typeof (xhr.status) != 'undefined' && xhr.status != null) {
                    sRetMsg = strDefault + ' Status: ' + xhr.status + ' ' + xhr.statusText;
                } else { sRetMsg = strDefault + '  ' + xhr.statusText; }
            } else if (typeof (xhr.xhr.status) != 'undefined' && xhr.xhr.status != null && xhr.xhr.status == '404') { sRetMsg = "API resource not found " + strDefault; }
        }
    }
    return sRetMsg;
}

function menuSettings() {
    $('#btnMenuSettings').hide();
    $('#btnMenuSettingsHide').show();
    $('.menuItem').toggle();
    $('.menuItem').css("display", "inline-block");
    $('#menuSettings').show();
    $('span.nonVisible').show();

    $('#menuTree li[style*="display: none"]').toggle();

    var menutree = $("#menuTree");
    var widget = kendo.widgetInstance(menutree);
    if (widget) {
        widget.options.dragAndDrop = true;
        $("#menuTree").data("kendoTreeView").setOptions(widget.options);
    }    

    var date = new Date();
    date.setTime(date.getTime() + (3 * 60 * 1000));
    var expires = "; expires=" + date.toGMTString();
    document.cookie = "menuOpen=true" + expires + "; path=/";

}

function menuReset() {
    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/MenuTree/ResetMenu',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function () {
            location.reload();
        }
    });
}

function menuSettingsHide() {
    $('#btnMenuSettings').hide();
    $('span.nonVisible').closest('li').hide();
    $('.menuItem').css("display", "inline-block");
    $('.menuItem').toggle();

    var menutree = $("#menuTree");
    var widget = kendo.widgetInstance(menutree);

    if (widget) {
        widget.options.dragAndDrop = false;

        $("#menuTree").data("kendoTreeView").setOptions(widget.options);
    }

    document.cookie = 'menuOpen=; Max-Age=0'

    $('#btnMenuSettingsHide').hide();
    $('#btnMenuSettings').show();
}

function menuTreeHighlightPage() {
    var treeView = $("#menuTree").data("kendoTreeView");
    if (typeof (treeView) !== 'undefined' && ngl.isObject(treeView)) {
        var barDataItem = treeView.dataSource.get(PageControl);
        //modified by RHR v-8.0 on 8/21/2017 fixed undefined error
        if (typeof (barDataItem) !== 'undefined' && ngl.isObject(barDataItem)) {
            var getNode = treeView.findByUid(barDataItem.uid);
            if (typeof (getNode) !== 'undefined' && ngl.isObject(getNode)) {
                //$(getNode).find("a").addClass("menuTreeCurrentPage");
                $(getNode).find("a:first").addClass("k-state-selected");
                $(getNode).find("a:first").removeClass("k-link");
            }
        }
    }
}

function getAlerts() {
    $.ajax({
        async: true,
        type: "GET",
        url: "api/Alert/Get365AlertMessagesForUser",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) != 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("Get Alerts Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                            blnSuccess = true;
                            if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined' && ngl.isObject(data.Data[0])) {
                                $.each(data.Data, function (i, an) {
                                    ngl.showAlertNotification(an.Title, an.Msg, null);
                                });
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Alerts Failure"; }
                    ngl.showErrMsg("Get Alerts Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure"); ngl.showErrMsg("Get Alerts Failure", sMsg, null); }
    });
    setTimeout(getAlerts, 60000);
}

function showHideUserPageDetail(iPageDetPageControl, iPageDetControl, bPageDetVisible, blnSettingsExist, kNGLGrid) {
    if (!blnSettingsExist) {
        //if this is the first time the user page settings are being created then start the spinner and show the create new settings message
        ngl.showInfoNotification("Creating Custom User Settings", "Please wait while custom user settings are created for the first time. The page will automatically reload when operation is complete", null);
        if (typeof (tPage["showPageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["showPageSpinner"])) { tPage["showPageSpinner"](kNGLGrid); }
    }
    setTimeout(function () {
        var PageDetUserColumnSetting = { PageDetPageControl: iPageDetPageControl, PageDetControl: iPageDetControl, PageDetColumnValue: bPageDetVisible };
        $.ajax({
            async: true,
            type: "POST",
            url: "api/cmPageDetail/PostPageDetUserVisibility",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(PageDetUserColumnSetting),
            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            success: function (data) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            },
            error: function (xhr, textStatus, error) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            }
        });
    }, 500); //we have to setTrimeout in order for the spinner to work properly (also the spinner works better when the ajax is async)
}


function UpdateUserPageDetailColumnWidth(iPageDetPageControl, iPageDetControl, iPageDetWidth, blnSettingsExist, kNGLGrid) {
    if (!blnSettingsExist) {
        //if this is the first time the user page settings are being created then start the spinner and show the create new settings message
        ngl.showInfoNotification("Creating Custom User Settings", "Please wait while custom user settings are created for the first time. The page will automatically reload when operation is complete", null);
        if (typeof (tPage["showPageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["showPageSpinner"])) { tPage["showPageSpinner"](kNGLGrid); }
    }
    setTimeout(function () {
        var PageDetUserColumnSetting = { PageDetPageControl: iPageDetPageControl, PageDetControl: iPageDetControl, PageDetColumnValue: iPageDetWidth };
        $.ajax({
            async: true,
            type: "POST",
            url: "api/cmPageDetail/PostPageDetUserColumnWidth",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(PageDetUserColumnSetting),
            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            success: function (data) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            },
            error: function (xhr, textStatus, error) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            }
        });
    }, 500); //we have to setTrimeout in order for the spinner to work properly (also the spinner works better when the ajax is async)
}

function UpdateUserPageDetailColumnSequence(iPageDetPageControl, iPageDetControl, iPageDetSequenceNo, blnSettingsExist, kNGLGrid) {
    if (!blnSettingsExist) {
        //if this is the first time the user page settings are being created then start the spinner and show the create new settings message
        ngl.showInfoNotification("Creating Custom User Settings", "Please wait while custom user settings are created for the first time. The page will automatically reload when operation is complete", null);
        if (typeof (tPage["showPageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["showPageSpinner"])) { tPage["showPageSpinner"](kNGLGrid); }
    }
    setTimeout(function () {
        var PageDetUserColumnSetting = { PageDetPageControl: iPageDetPageControl, PageDetControl: iPageDetControl, PageDetColumnValue: iPageDetSequenceNo };
        $.ajax({
            async: true,
            type: "POST",
            url: "api/cmPageDetail/PostPageDetUserColumnSequence",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(PageDetUserColumnSetting),
            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            success: function (data) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            },
            error: function (xhr, textStatus, error) {
                if (!blnSettingsExist) {
                    //if this is the first time the user page settings are being created then stop the spinner and reload the page (otherwise the usercontrol will still be 0 and it will keep showing the create new settings message)
                    if (typeof (tPage["hidePageSpinner"]) !== 'undefined' && ngl.isFunction(tPage["hidePageSpinner"])) { tPage["hidePageSpinner"](kNGLGrid); }
                    document.location = oredirectUri;
                }
            }
        });
    }, 500); //we have to setTrimeout in order for the spinner to work properly (also the spinner works better when the ajax is async)
}

function showPageSpinner(kNGLGrid) { kendo.ui.progress(kNGLGrid, true); }
function hidePageSpinner(kNGLGrid) { kendo.ui.progress(kNGLGrid, false); }


//kendo widget extensions
//custom kendo grid
// Modified by CHA v-8.5 on 06/22/2021
// Added an option to drag and drop when you set the option nglCustomDragAndDrop to true which is passed by:
// (string.IsNullOrEmpty(pdh.PageDetFieldFormatOverride) && pdh.PageDetFieldFormatOverride.ToLower() == "draganddrop" ? "'false'" : "'true'")
(function ($, kendo) {

    // Make table rows of the grid draggable 
    function attachDraggable(input) {
        let that = input;
        let gridObj = that.wrapper[0];

        let gridOptions = that.options;
        let callback;
        if (gridOptions.nglDragStart && typeof (gridOptions.nglDragStart) === typeof (Function)) {
            callback = gridOptions.nglDragStart;
        }

        if (gridObj) {
            $(gridObj).kendoDraggable({
                filter: "tbody>tr>td.k-command-cell",
                hint: function (e) {
                    var item = $('<div class="k-grid k-widget" style="background-color: DarkOrange; color: black;"><table><tbody><tr>' + e.parent().html() + '</tr></tbody></table></div>');
                    return item;
                },
                dragstart: function (e) {
                    let draggedObject;
                    let parent = $(e.currentTarget[0]).parent()[0];
                    if (parent) {
                        var id = $(parent).data("uid");
                        if (id) {
                            draggedObject = that.dataSource.getByUid(id);
                            window.NgLDragDataObj = draggedObject;
                        }
                    }

                    if (callback) {
                        let event = e;
                        callback(draggedObject, event);
                    }
                },
            });
        }               
    }

    function atachDropTarget() {
        let gridId = this._cellId.split('_')[0];
        let gridOptions = this.options;
        let callback;
        if (gridOptions.nglDragEnd && typeof (gridOptions.nglDragEnd) === typeof (Function)) {
            callback = gridOptions.nglDragEnd;
        }
        let selector = `#${gridId} tr.k-master-row`;
        let that = this;
        $(selector).kendoDropTarget({
            drop: function (e) {
                if (callback) {
                    let droppedObjectId = $(e.dropTarget[0]).data("uid");
                    // Modified by CHA on 09/30/2021
                    // Finding the index of each of the object in the dataSource of the grid
                    let droppedObjectPosition = that.dataSource._data.map(el => el.uid).indexOf(droppedObjectId);
                    let targetObjectPosition = that.dataSource._data.map(el => el.uid).indexOf(window.NgLDragDataObj.uid);
                    // Calculating if we are draggig up or down 
                    // If isDraggingUp == false this would mean we are going down, otherwise is up 
                    let isDraggingUp = droppedObjectPosition < targetObjectPosition;

                    let droppedObject = that.dataSource.getByUid(droppedObjectId);
                    callback(window.NgLDragDataObj, droppedObject, isDraggingUp);
                }
            }
        });       
    }

    var NGLGrid = kendo.ui.Grid.extend({
        options: { toolbarColumnMenu: false, name: "NGLGrid" },
        init: function (element, options) {            
            // Initialize the widget.
            if (options.toolbarColumnMenu === true && typeof options.toolbar === "undefined") { options.toolbar = []; }            

            // init the widget
            kendo.ui.Grid.fn.init.call(this, element, options);

            // Call the base class init.
            this._initToolbarColumnMenu();

            // Add custom option to figure out if the drag and drop is enabled
            if (options.nglCustomDragAndDrop) {
                // we need the wrapper to exist so move down the function in the execution order
                attachDraggable(this);
                this._events.dataBound.push(atachDropTarget);
            }           
        },
        _initToolbarColumnMenu: function () {
            var kNGLGrid = this.element;
            // Determine whether the column menu should be displayed, and if so, display it.
            // The toolbar column menu should be displayed.
            if (this.options.toolbarColumnMenu === true && this.element.find(".k-ext-grid-columnmenu").length === 0) {
                // Create the column menu items.
                var $menu = $("<ul style='height:200px; left:-20px;'></ul>");
                // Make a copy of the columns to loop over and add them to the column menu.
                var cList = this.columns.slice();  // slice makes a shallow copy,  the array is a different list of the same objects by reference
                //sort the cList by title while leaving the original sort in  columns unmodified
                cList.sort(function (a, b) {
                    let keyA = a.title;
                    let keyB = b.title;
                    if (keyA < keyB) return -1;
                    if (keyA > keyB) return 1;
                    return 0;
                });
                //for (var idx = 0; idx < this.columns.length; idx++) {
                for (var idx = 0; idx < cList.length; idx++) {
                    var column = cList[idx];
                    // A column must have a title to be added.
                    if ($.trim(column.title).length > 0 && column.showhide == 1) {
                        // Add columns to the column menu.
                        $menu.append(kendo.format("<li style='max-width:200px;' ><input  type='checkbox' data-index='{0}' data-field='{1}' data-title='{2}' {3}>{4}</li>",
                            idx, column.field, column.title, column.hidden ? "" : "checked", column.title));
                    }
                }
                // <span  style='float:right; width:20px;'>&nbsp;</span>
                // Create a "Columns" menu for the toolbar.
                this.wrapper.find("div.k-grid-toolbar").css("overflow", "visible").append("<span  style='float:right; width:50px;'>&nbsp;</span><ul class='k-ext-grid-columnmenu' style='float:right; margin-left:auto; margin-right:50px'><li data-role='menutitle'><span class='k-icon k-i-gears'></span></li></ul>"); //Grid Columns -- <span class="k-icon k-i-gears"></span>
                this.wrapper.find("div.k-grid-toolbar ul.k-ext-grid-columnmenu li").append($menu);
                var that = this;
                this.wrapper.find("div.k-grid-toolbar ul.k-ext-grid-columnmenu").kendoMenu({
                    closeOnClick: false,
                    select: function (e) {
                        // Get the selected column.
                        var $item = $(e.item), $input, columns = that.columns;
                        $input = $item.find(":checkbox");
                        if ($input.attr("disabled") || $item.attr("data-role") === "menutitle") { return; }
                        var column = that._findColumnByTitle($input.attr("data-title"));
                        if (typeof (column) != "undefined" && column != null) {
                            try {
                                var iPageDetPageControl = column.PageDetPageControl;
                                var iPageDetControl = column.PageDetControl;
                                var iPageDetUSC = column.PageDetUserSecurityControl;
                                var blnSettingsExist = true;
                                if (iPageDetUSC === 0) { blnSettingsExist = false; } else { blnSettingsExist = true; }
                                // If checked, then show the column; otherwise hide the column.
                                if ($input.is(":checked")) {
                                    that.showColumn(column);
                                    //save the user settings
                                    showHideUserPageDetail(iPageDetPageControl, iPageDetControl, true, blnSettingsExist, kNGLGrid)
                                } else {
                                    that.hideColumn(column);
                                    //save the user settings
                                    showHideUserPageDetail(iPageDetPageControl, iPageDetControl, false, blnSettingsExist, kNGLGrid)
                                }
                            } catch (err) {
                                //do nothing
                            }
                        }
                    }
                });
            }
        },
        _findColumnByTitle: function (title) {
            // Find a column by column title.
            var result = null;
            for (var idx = 0; idx < this.columns.length && result === null; idx++) {
                column = this.columns[idx];
                if (column.title === title) { result = column; }
            }
            return result;
        },
        _capturegroup: function (e) {
            if (e.groups.length) {
                var sgroups = JSON.stringify(e.groups)
                alert(sgroups);
                //alert(e.groups[0].dir);
                //console.log(e.groups[0].field);
                //console.log(e.groups[0].dir);
            }
        },
        _captureColumnResize: function (e) {
            var column = e.column
            if (typeof (column) != "undefined" && column != null) {
                try {
                    var iNewWidth = e.newWidth;
                    if (!iNewWidth) { return; }
                    if (iNewWidth > 0) {
                        var iPageDetPageControl = column.PageDetPageControl;
                        var iPageDetControl = column.PageDetControl;
                        var iPageDetUSC = column.PageDetUserSecurityControl;
                        var blnSettingsExist = true;
                        if (iPageDetUSC === 0) { blnSettingsExist = false; } else { blnSettingsExist = true; }
                        UpdateUserPageDetailColumnWidth(iPageDetPageControl, iPageDetControl, iNewWidth, blnSettingsExist, this.element);
                    }
                } catch (err) {
                    //do nothing
                }
            }
            //alert("Field: " +  e.column.field + " Width: " + e.newWidth.toString() + " Old Width: " + e.oldWidth.toString());
        },
        _captureColumnReorder: function (e) {
            var column = e.column
            if (typeof (column) != "undefined" && column != null) {
                try {
                    var iNewIndex = e.newIndex;
                    if (!iNewIndex) { return; }
                    if (iNewIndex > 0) {
                        var iPageDetPageControl = column.PageDetPageControl;
                        var iPageDetControl = column.PageDetControl;
                        var iPageDetUSC = column.PageDetUserSecurityControl;
                        var blnSettingsExist = true;
                        if (iPageDetUSC === 0) { blnSettingsExist = false; } else { blnSettingsExist = true; }
                        UpdateUserPageDetailColumnSequence(iPageDetPageControl, iPageDetControl, iNewIndex, blnSettingsExist, this.element);
                    }
                } catch (err) {
                    //do nothing
                }
            }
            //alert("Field: " +  e.column.field + " Width: " + e.newIndex.toString() + " Old Width: " + e.oldIndex.toString());
        }
    });
    kendo.ui.plugin(NGLGrid);
})(window.kendo.jQuery, window.kendo);

function onDragEndMenuTree(e) {
    let treeView = $("#menuTree").data("kendoTreeView");

    if (treeView.dataItem(treeView.parent(e.sourceNode))?.text === undefined) {

        let itemsLength = $(e.sourceNode).find("div.menuItem").length;

        for (var i = 0; i < itemsLength; i++) {

            let el = $(e.sourceNode).find("div.menuItem")[i];

            $($(el).parent().parent().parent()[0]).show();

            $(el).parent().find('span[name="itemName"]').show();

            $(el).css("display", "inline-block");

            $(el).find('span[name="Remove"]').hover(function (e) {
                $(el).find('span[name="Remove"]').parents("a.k-in").removeClass("k-fab k-fab-error");
            });

            $(el).find('span[name="Remove"]').mouseover(function (e) {
                $(el).find('span[name="Remove"]').parents("a.k-in").addClass("k-fab k-fab-error");
            });

            $(el).find('span[name="Favorite"]').hover(function (e) {
                $(el).find('span[name="Favorite"]').parents("a.k-in").removeClass("k-fab k-fab-warning");
            });

            $(el).find('span[name="Favorite"]').mouseover(function (e) {
                $(el).find('span[name="Favorite"]').parents("a.k-in").addClass("k-fab k-fab-warning");
            });

            $(el).find('span[name="Restore"]').hover(function (e) {
                $(el).find('span[name="Restore"]').parents("a.k-in").removeClass("k-fab k-fab-success");
            });

            $(el).find('span[name="Restore"]').mouseover(function (e) {
                $(el).find('span[name="Restore"]').parents("a.k-in").addClass("k-fab k-fab-success");
            });
        }
        return;
    }

    var el = $(e.sourceNode).find("div.menuItem")[0];

    $(el).css("display", "inline-block");

    $(el).find('span[name="Remove"]').hover(function () {
        $(el).find('span[name="Remove"]').parents("a.k-in").removeClass("k-fab k-fab-error");
    });

    $(el).find('span[name="Remove"]').mouseover(function () {
        $(el).find('span[name="Remove"]').parents("a.k-in").addClass("k-fab k-fab-error");
    });

    $(el).find('span[name="Favorite"]').hover(function () {
        $(el).find('span[name="Favorite"]').parents("a.k-in").removeClass("k-fab k-fab-warning");
    });

    $(el).find('span[name="Favorite"]').mouseover(function () {
        $(el).find('span[name="Favorite"]').parents("a.k-in").addClass("k-fab k-fab-warning");
    });

    $(el).find('span[name="Restore"]').hover(function () {
        $(el).find('span[name="Restore"]').parents("a.k-in").removeClass("k-fab k-fab-success");
    });

    $(el).find('span[name="Restore"]').mouseover(function () {
        $(el).find('span[name="Restore"]').parents("a.k-in").addClass("k-fab k-fab-success");
    });
    $('li[style*="display: none"]').show();
    $(el).prev().show();
}

function addMenuItemToFavorites(favoritesId, itemId) {
    var treeView = $("#menuTree").data("kendoTreeView");
    var data = treeView?.dataSource?.data();
    var fav = data.find(e => e.text.includes('Favorites'));
    var newPosition = (fav?.items?.length || 0) + 1;

    $.ajax({
        async: true,
        type: 'POST',
        url: `/api/MenuTree/AddMenuItemToFavorites?favoritesId=${favoritesId}&itemId=${itemId}&itemPosition=${newPosition}`,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function () {
            location.reload();
        }
    });
    e.preventDefault();
}

function removeMenuItemFromFavorites(itemId) {
    $.ajax({
        async: true,
        type: 'POST',
        url: `/api/MenuTree/RemoveMenuItemFromFavorites?itemId=${itemId}`,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function () {
            location.reload();
        }
    });
    e.preventDefault();
}

function dropNodeMenuTree(e) {
    var treeView = $("#menuTree").data("kendoTreeView");
    if (e.sourceNode !== undefined && e.destinationNode !== undefined) {
        if (e.dropPosition === "over" || (treeView.dataItem(treeView.parent(e.sourceNode))?.text !== treeView.dataItem(treeView.parent(e.destinationNode))?.text)) {
            e.setValid(false);
        }

        if (treeView.dataItem(treeView.parent(e.sourceNode))?.text === undefined) {
            let security = $($(e.sourceNode).find("[name='itemName']")[0]).data("security") | 0;
            let oldPosition = $($(e.sourceNode).find("[name='itemName']")[0]).data("order") | 0;
            let newPosition = $($(e.destinationNode).find("[name='itemName']")[0]).data("order") | 0;
            let itemId = $($(e.sourceNode).find("[name='itemName']")[0]).data("itemid") | 0;

            if (newPosition !== oldPosition) {
                $.ajax({
                    async: true,
                    type: 'POST',
                    url: `/api/MenuTree/ReorderRootMenu?userSecurityControl=${security}&itemId=${itemId}&position=${newPosition}`,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                });
            }
            return;
        }

        let parentId = $($(e.sourceNode).find("[name='itemName']")[0]).data("parentid") | 0;
        let itemId = $($(e.sourceNode).find("[name='itemName']")[0]).data("itemid") | 0;
        let oldPosition = $($(e.sourceNode).find("[name='itemName']")[0]).data("position") | 0;
        let newPosition = $($(e.destinationNode).find("[name='itemName']")[0]).data("position") | 0;

        if (newPosition !== oldPosition) {
            $.ajax({
                async: true,
                type: 'POST',
                url: `/api/MenuTree/ReorderMenu?parentId=${parentId}&itemId=${itemId}&position=${newPosition}`,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
            });
        }
    }
}

function onMenuTreeDataBound() {
    var treeView = $("#menuTree").data("kendoTreeView");
    var data = treeView?.dataSource?.data();

    data?.forEach(e => {
        if (e.children?.options?.data?.items) {
            e.children.options.data.items.forEach(c => {
                if (!c.visible) {
                    var regexpWithoutE = /data-itemId="(.*)" data/;
                    var id = c.text.match(regexpWithoutE)[1];
                    if (id) {
                        var el1 = $(`span[data-itemId='${c.text.match(regexpWithoutE)[1]}']`);
                        el1.closest("li.k-item").hide();
                    }
                }
            })
        }
    });
}

function toggleMenuItemVisibility(itemId) {
    if (itemId) {
        $.ajax({
            async: true,
            type: 'POST',
            url: '/api/MenuTree/ToggleMenuItemVisibility/' + itemId,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            success: function () {
                location.reload();
            }
        });
    }
    e.preventDefault();
}

function dragNodeMenuTree(e) {
    if (e.statusClass && e.statusClass === "i-plus") {
        e.setStatusClass("k-i-cancel");
    }
}

function saveExpandNode(e) {
    var dataItem = e.sender.dataItem(e.node);
    var intMTC = dataItem.mtc;
    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/MenuTree/ExpandNode/' + intMTC,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
    });
}

function saveCollapseNode(e) {
    var dataItem = e.sender.dataItem(e.node);
    var intMTC = dataItem.mtc;
    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/MenuTree/CollapseNode/' + intMTC,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
    });
}

function menuItemHover() {
    $(document).ready(function () {
        $("span .k-i-close-circle").hover(function () {
            $(this).css("background-color", "yellow");
        });
    });
}

function resetCurrentUserConfig(iPageControl) {
    $.ajax({
        async: false,
        type: "DELETE",
        url: "api/cmPageDetail/ResetCurrentUserConfig/" + iPageControl,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
        success: function (data) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) != 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        blnErrorShown = true;
                        ngl.showErrMsg("ResetCurrentUserConfig Failure", data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined') {
                                blnSuccess = true;
                                document.location = oredirectUri;
                            }
                        }
                    }
                }
                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = "ResetCurrentUserConfig Failure"; }
                    ngl.showErrMsg("ResetCurrentUserConfig Failure", strValidationMsg, null);
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
        },
        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("ResetCurrentUserConfig Failure", sMsg, null); }
    });
}

function openInNewTab(url) {
    var win = window.open(url, '_blank');
    win.focus();
}


////readGetPageSettingSuccessCallback = function (data) {
////    var oResults = new nglEventParameters();
////    var tObj = this;
////    oResults.source = "readGetPageSettingSuccessCallback";
////    oResults.msg = 'Failed'; //set default to Failed         
////    oResults.CRUD = "read";
////    oResults.widget = tObj;
////    var blnDeleteUserSettings = false;
////    //////clear any old return data in rData
////    ////this.rData = null;
////    ////try {
////    ////    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////    ////        if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
////    ////            if (this.ReadUserSettings === true) {
////    ////                oResults.error = new Error();
////    ////                oResults.error.name = "Read " + this.DataSourceName + " Settings Failure";
////    ////                oResults.error.message = data.Errors;
////    ////                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////    ////            } else {
////    ////                blnDeleteUserSettings = true;
////    ////            }
////    ////        }
////    ////        else {
////    ////            if (this.ReadUserSettings === true) {
////    ////                this.rData = data.Data;
////    ////                if (data.Data != null) {
////    ////                    oResults.datatype = "WorkFlowSetting";
////    ////                    oResults.msg = "Success"
////    ////                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data) && ngl.isObject(data.Data[0]) && data.Data[0].value !== null) {
////    ////                        this.workFlowSettings = JSON.parse(data.Data[0].value);
////    ////                        oResults.data = this.workFlowSettings
////    ////                    }
////    ////                }
////    ////                else {
////    ////                    oResults.error = new Error();
////    ////                    oResults.error.name = "Invalid Request";
////    ////                    oResults.error.message = "No Data available.";
////    ////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////    ////                }
////    ////            } else {
////    ////                blnDeleteUserSettings = true;
////    ////                oResults.datatype = "WorkFlowSetting";
////    ////                oResults.msg = "Success"
////    ////            }
////    ////        }
////    ////    } else {
////    ////        oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
////    ////        ngl.showSuccessMsg(oResults.msg, tObj);
////    ////    }
////    ////} catch (err) {
////    ////    oResults.error = err
////    ////}
////    ////if (this.ReadUserSettings !== true || oResults.msg !== "Success") {
////    ////    this.loadDeaultWorkFlows();
////    ////    oResults.data = this.workFlowSettings
////    ////}
////    ////if (blnDeleteUserSettings !== true) {
////    ////    tObj.deleteUserSettings();
////    ////}
////    ////if (ngl.isFunction(tPage[this.callback])) {
////    ////    tPage[this.callback](oResults);
////    ////}
////    ////if (oResults.msg == "Success") {
////    ////    this.show();
////    ////}
////    var dsUserPageSettings = null;
////    var sKey = "ManageSchedulePage";
////    $.ajax({
////        url: '/api/ManageSchedule/GetPageSettings/' + sKey,
////        contentType: 'application/json; charset=utf-8',
////        dataType: 'json',
////        async: false,
////        data: { filter: sKey },
////        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
////        success: function (data) {
////            //debugger;
////            dsUserPageSettings = data.Data[0];
////            try {
////                var blnSuccess = false;
////                var blnErrorShown = false;
////                var strValidationMsg = "";
////                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
////                        blnErrorShown = true;
////                        ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null);
////                    }
////                    else {
////                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
////                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
////                                blnSuccess = true;
////                            }
////                        }
////                    }
////                }
////                if (blnSuccess === false && blnErrorShown === false) {
////                    if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to recieve thsi message."; }
////                    ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);
////                }
////            } catch (err) {
////                ngl.showErrMsg(err.name, err.description, null);
////            }
////        }
////    });
////}

////readGetPageSettingAjaxErrorCallback = function (xhr, textStatus, error) {
////    var oResults = new nglEventParameters();
////    var tObj = this;
////    oResults.source = "readGetPageSettingAjaxErrorCallback";
////    oResults.msg = 'Failed'; //set default to Failed        
////    oResults.CRUD = "read"
////    oResults.widget = tObj;
////    oResults.error = new Error();
////    oResults.error.name = "Read Page Settings Failure"
////    oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////}

//The caller page must implement readGetPageSettingSuccessCallback() and readGetPageSettingAjaxErrorCallback()
function getPageSettings(tObj, controllerName, pageSettingName, doasync) {
    var oCRUDCtrl = new nglRESTCRUDCtrl();
    var url = controllerName + "/GetPageSettings"
    //var sFilter = pageSettingName;
    var blnRet = oCRUDCtrl.filteredRead(controllerName + "/GetPageSettings", pageSettingName, tObj, "readGetPageSettingSuccessCallback", "readGetPageSettingAjaxErrorCallback", doasync);
    return blnRet;
}

//The caller page must implement savePostPageSettingSuccessCallback() and savePostPageSettingAjaxErrorCallback()
function postPageSetting(tObj, controllerName, UserPageSettingsData, doasync) {
    var oCRUDCtrl = new nglRESTCRUDCtrl();
    var url = controllerName + "/PostPageSetting"
    var blnRet = oCRUDCtrl.update(url, UserPageSettingsData, tObj, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", doasync);
    return blnRet;
}

function formatPhoneNumber(phoneNumberString) {
    //return 'nbr = ' + phoneNumberString;
    var cleaned = ('' + phoneNumberString).replace(/\D/g, '')
    var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/)
    if (match) { return '(' + match[1] + ') ' + match[2] + '-' + match[3]; }
    return '';
}

//function numberWithSpaces(value, pattern) {
//    var i = 0,
//      phone = value.toString();
//    return pattern.replace(/#/g, _ => phone[i++]);
//}

//var n;
//var p;
//var p1;
//function ValidatePhone(){
//    p=p1.value
//    if(p.length==3){
//        //d10=p.indexOf('(')
//        pp=p;
//        d4=p.indexOf('(')
//        d5=p.indexOf(')')
//        if(d4==-1){
//            pp="("+pp;
//        }
//        if(d5==-1){
//            pp=pp+")";
//        }
//        //pp="("+pp+")";
//        document.frmPhone.txtphone.value="";
//        document.frmPhone.txtphone.value=pp;
//    }
//    if(p.length>3){
//        d1=p.indexOf('(')
//        d2=p.indexOf(')')
//        if (d2==-1){
//            l30=p.length;
//            p30=p.substring(0,4);
//            //alert(p30);
//            p30=p30+")"
//            p31=p.substring(4,l30);
//            pp=p30+p31;
//            //alert(p31);
//            document.frmPhone.txtphone.value="";
//            document.frmPhone.txtphone.value=pp;
//        }
//    }
//    if(p.length>5){
//        p11=p.substring(d1+1,d2);
//        if(p11.length>3){
//            p12=p11;
//            l12=p12.length;
//            l15=p.length
//            //l12=l12-3
//            p13=p11.substring(0,3);
//            p14=p11.substring(3,l12);
//            p15=p.substring(d2+1,l15);
//            document.frmPhone.txtphone.value="";
//            pp="("+p13+")"+p14+p15;
//            document.frmPhone.txtphone.value=pp;
//            //obj1.value="";
//            //obj1.value=pp;
//        }
//        l16=p.length;
//        p16=p.substring(d2+1,l16);
//        l17=p16.length;
//        if(l17>3&&p16.indexOf('-')==-1){
//            p17=p.substring(d2+1,d2+4);
//            p18=p.substring(d2+4,l16);
//            p19=p.substring(0,d2+1);
//            //alert(p19);
//            pp=p19+p17+"-"+p18;
//            document.frmPhone.txtphone.value="";
//            document.frmPhone.txtphone.value=pp;
//            //obj1.value="";
//            //obj1.value=pp;
//        }
//    }
//    //}
//    setTimeout(ValidatePhone,100)
//}
//function getIt(m){
//    n=m.name;
//    //p1=document.forms[0].elements[n]
//    p1=m
//    ValidatePhone()
//}
//function testphone(obj1){
//    p=obj1.value
//    //alert(p)
//    p=p.replace("(","")
//    p=p.replace(")","")
//    p=p.replace("-","")
//    p=p.replace("-","")
//    //alert(isNaN(p))
//    if (isNaN(p)==true){
//        alert("Check phone");
//        return false;
//    }
//}
////  End -->
