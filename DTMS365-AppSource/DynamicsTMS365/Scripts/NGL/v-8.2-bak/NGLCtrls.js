//Note each of the NGL Ctrls uses an array of DataFieldDetails records
// se NGLObjects.js for the definition fo the object
//  to determine how the control is displayed
// each record contains the following fields
// fieldID: 0; fieldTagID: "";fieldCaption: ""; fieldName: ""; fieldDefaultValue: ""; fieldGroupSubType: 0;fieldReadOnly: true; fieldFormat: "";fieldTemplate: "";fieldAllowNull: true; fieldVisible: true; fieldRequired: true; fieldMaxlength: "50";  fieldInsertOnly: false; fieldAPIReference: ""; fieldAPIFilterID: ""; fieldParentTagID: 0
// Note that the NGL Controls expect global variables to be declared in each page
//  The code will attempt to use defaults if the variables are missing (when required by the contro)
//  but the control may not be fully functional,  for example if the control needs the PageControl number 
//  and this value is undefined or null the control or api may fail to execute.
//  Here is a list of the standard variables that may be used by the controls
//  var PageControl // the page control number in cmPage
//  var tPage a reference to the page level DOM object for the page (this) 
// call backs
// each control has a parent window or page header object  one callback function is 
// create for each using the PageDetName + the letters CB.  pages should implemnt this
// function to process call back data.  Call back information details is returned via
// the nglEventParameters parameter.  the implementaiton of the call back function 
// should check the nglEventParameters.source property to determine what method of the 
// control triggered the call back. a reference to the control is returned in the widget property
// and a reference to any valid data is returned in the data property as an array
// look at the NGL control object definition for details but some common sources are:
// . saveSuccessCallback
// . saveAjaxErrorCallback
// . readSuccessCallback
// . readAjaxErrorCallback
// . deleteSuccessCallback
// . deleteAjaxErrorCallback
function NGLPopupWindCtrl() {

    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    //Widget specific properties
    this.DataSourceName = "Record";
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    this.AddErrorMsg = "Invalid field information; cannot add new record";
    this.AddErrorTitle = "Field Data Required";
    screenLEName: null;
    IDKey: "id1234";
    ParentIDKey: "id22222";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    this.kendoWindowsObjUploadEventAdded = 0;
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.PKName = "";
    this.sNGLCtrlName = ""; //variable used on page for the control like "wdgt" +  sCleanPageDetName + "Dialog"
    this.ctrlSubTypes = new GroupSubTypes();
    this.ctrlContainers = null; //variable to hold a list of all the containers that may expose CRUD  and Action operations.  the popup control will call these methods using this list
    this.readKey = 0;
   
    //Widget specific functions
    this.ListChanged = function (e, tList) {
        var source = this;
        //NGLPopupWindCtrl
        if (typeof (NGLPopupWindCtrlListChanged) !== 'undefined' && ngl.isFunction(NGLPopupWindCtrlListChanged)) {
            NGLPopupWindCtrlListChanged(e, tList, source);
        }
    }
            
    this.clearWdgtHTML = function () {
        //TODO: add logic to loop through other containers for future
        if (this.ctrlContainers != null) {
            $.each(this.ctrlContainers, function (index, item) {                
                //currently only some objects support saving
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                    var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined' && ngl.isObject(tPage[sControlVariableName])) {
                        tPage[sControlVariableName].clearWdgtHTML();
                    }
                } 
            });
        }
        
    }

    this.GetFieldID = function(sName){
        var sRet = "";
        if(!sName) { return sRet;}
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName){
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.SetFieldDefault = function (sName,sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItem = function (sID){
        var sRet = null;
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }
    
    this.GetFieldItemByFieldID = function (sFieldID){
        var sRet = null;
        if (!sFieldID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldID == sFieldID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }
    
    this.loadHTML = function () {
        var tObj = this;
        tObj.blnEditing = true; //we are always editing the popup control we do not perform CRUD operations directly
        if (typeof (this.ctrlSubTypes) === "undefined" || !ngl.isObject(this.ctrlSubTypes)) {this.ctrlSubTypes = new GroupSubTypes();}
        this.ctrlSubTypes.addDefaultSubTypesIfNeeded();
        var divContainer = $("#" + this.ContainerDivID);
        divContainer.css({position: "relative"})
        if (!divContainer) { return false; }
        //divContainer.html(""); //clear any existing html data
        //var divWindowID = "wnd" + this.IDKey + "Dialog";
        var divThisWrapperID = "div" + this.IDKey +"wrapper";
        //divContainer.append(kendo.format("<div id='{0}'></div>", divWindowID));
        //var divWindow = $("#" + divWindowID);
        var divActive = divContainer;
        divActive.append('<a id="' + this.IDKey + 'focusCancel" href="#"></a>');
        var divEdit = $("#" + divThisWrapperID);
        divActive.append(kendo.format("<div id='{0}' style='position:relative;'></div>", divThisWrapperID));
        $("#" + divThisWrapperID).append("<div id='div" + this.IDKey + "wait' style='position:relative; display:none;' ><span style='vertical-align:middle;'>Please Wait Loading&nbsp;</span><img style='vertical-align:middle;' border='0' alt='Waiting' src='../Content/NGL/loading5.gif' ></div>");
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var divWrapper =  $("#" + divThisWrapperID);
            divWrapper.append(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));
            divActive = divWrapper;

            var $HiddenFields = $("<div style='display:none;'></div>"); 
            var $InsertOnlyHTML =  $("#div" + this.IDKey + "InsertOnly");  //$(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));
            var $floattable = $("<table class='tblResponsive'></table>");
            var $floattableheaderrow =  $("<tr></tr>");
            var $floattableitemrow =  $("<tr></tr>");
            var floattablecount = 0;
            var sRequiredAsterik = '';
            var sReadOnly = '';
            var bDivOpen = false;
            var bFloatLeftTableOpen = false;
            var bFloatHeadersOpen = false;
            var sFloatTableHeader = "";
            var sFloatTableRow = "";
            var iTableColumns = 0;
            var iTableHeaders = 0;
            var sFormated = "";
            var bInsertOnlyOpen = false;           
            var domLastID = divThisWrapperID;
            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                
                sFormated = "";
                if (item.fieldReadOnly === true) {
                    sReadOnly = '<span class="k-icon k-i-lock"></span>&nbsp;';
                    sRequiredAsterik = '';
                } else {
                    sReadOnly = '';
                    //if (item.fieldAllowNull === false || item.fieldRequired === true) {
                    //Modified by RHR for v-8.2 on 01/02/2019 removed logic to test for allow null on required
                    if (item.fieldRequired === true) {
                        sRequiredAsterik = '<span class="redRequiredAsterik"> *</span>';
                    }
                    else {
                        sRequiredAsterik = '';
                    }
                }
                //any child containers created by Content Management must load any required html into a temp container
                var sTmpChildContainerDivID = "tmp" + item.fieldTagID + "wrapper";
                var divTmpContainer = $("#" + sTmpChildContainerDivID);
                if (typeof (divTmpContainer) !== 'undefined'  && divTmpContainer != null ){
                    //add the divTmpContainer html to this container the child container does not exist
                    var sChildContainer = "div" + item.fieldTagID + "wrapper";
                    var divChildContainer = $("#" + sChildContainer);
                    if (!divChildContainer){
                        var sChildHTML = divTmpContainer.html();
                        divChildContainer.html(sChildHTML);
                        divTmpContainer.html("");
                        divActive.append(divChildContainer);
                    }
                }
                //check if this is a container and add it to the list.
                if (tObj.ctrlSubTypes.isSubTypeAContainer(item.fieldGroupSubType)) {
                    if (tObj.ctrlContainers == null) {
                        tObj.ctrlContainers = new Array();
                    }
                    tObj.ctrlContainers.push(item);
                }
                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  hide hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else {
                    if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                        var divChildWrapperID = "div" + item.fieldTagID + "wrapper";
                        var divChild = $("#" + divChildWrapperID);
                        if (divChild) {
                            var domLast = $("#" + domLastID);
                            if (domLast) {
                                divChild.insertAfter(domLast);
                            }
                            domLastID = divChildWrapperID;
                        }
                        //var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                        //if (typeof (tPage[sControlVariableName]) !== 'undefined' && ngl.isObject(tPage[sControlVariableName])) {
                        //    tPage[sControlVariableName].read();
                        //}
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Grid) {
                        var divGrid = $("#" + item.fieldTagID + "wrapper");
                        if (divGrid) {
                            var domLast = $("#" + domLastID);
                            if (domLast) {
                                divGrid.insertAfter(domLast);
                            }
                            domLastID = item.fieldTagID + "wrapper";
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Span) {
                        sFormated = kendo.format("<span id='sp{3}' style='{0}' class='{1}'>{2}</span>", item.fieldStyle, item.fieldCssClass, item.fieldCaption, item.fieldTagID);
                        domLastID = "sp" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Button) {
                        var sAction = "";
                        if (!isEmpty(item.fieldWidgetAction)) {
                            sAction = 'tPage["' + tObj.sNGLCtrlName + '"].raiseAction("' + item.fieldWidgetAction + '")';
                        }
                        if (item.fieldCssClass === 'cm-icononly-button' || item.fieldCssClass === 'k-button') {
                            if (isEmpty(item.fieldStyle)) {
                                item.fieldStyle = 'k-icon k-i-connector';
                            }
                            sFormated = kendo.format("&nbsp;&nbsp;<a id='btn{5}' class='{0}' href='#' onclick='{1}'><span class='{2}' style='{3}'></span>{4}</a>&nbsp;&nbsp;", item.fieldCssClass, sAction, item.fieldStyle, item.fieldFormat, item.fieldCaption, item.fieldTagID);
                        } else {
                            sFormated = kendo.format("&nbsp;&nbsp;<button id='btn{4}' class='{0}' style='{2}' onclick='{1}'>{3}</button>&nbsp;&nbsp;", item.fieldCssClass, sAction, item.fieldStyle, item.fieldCaption, item.fieldTagID);
                        }
                        domLastID = "btn" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header1) {
                        sFormated = kendo.format(" <h1 id='h1{0}' style='clear:both; display: block; float: none;'>{1}</h1>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h1" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header2) {
                        sFormated = kendo.format(" <h2 id='h2{0}' style='clear:both; display: block; float: none;'>{1}</h2>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h2" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header3) {
                        sFormated = kendo.format(" <h3 id='h3{0}' style='clear:both; display: block; float: none;'>{1}</h3>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h3" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header4) {
                        sFormated = kendo.format(" <h4 id='h4{0}' style='clear:both; display: block; float: none;'>{1}</h4>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h4" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Paragraph) {
                        sFormated = kendo.format(" <p id='p{0}' style='clear:both; display: block; float: none;'>{1}</p>", item.fieldTagID, item.fieldCaption);
                        domLastID = "p" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Link) {
                        sFormated = kendo.format(" <a id='a{0}' href='{2}'>{1}</a>", item.fieldTagID, item.fieldCaption, item.fieldAPIReference);
                        domLastID = "a" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLSpace) {
                        sFormated = "&nbsp;";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLLineBreak) {
                        sFormated = kendo.format(" <br id='br{0}' style='clear:both; display: block; float: none;' />", item.fieldTagID);
                        domLastID = "br" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Div) {
                        //once a div is entered it becomes active and everything below it falls inside this div until   
                        //the next float block, float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because divs are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divActive.append("<div id='div" + item.fieldTagID + "Break' style='padding: 0px 10px 10px 10px; clear:both; display: block; float: none;'></div>");
                        divActive = $("#div" + item.fieldTagID + "Break");
                        domLastID = "div" + item.fieldTagID + "Break";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatLeftTable) {
                        //Note: Once FloatLeftTable is active all fields are added to the table until a 
                        //div a FloatBlockLeft or another FloatLeftTable is activated
                        // fields are added to rows according to the number of tableheader fields included
                        // Labels are used to include text in a row instead of a data object                       
                        if (bFloatLeftTableOpen == true) {
                            //append the open float table to the active div and create a new float table
                            //float tables are always nested inside a <div style="float: left;">
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                        }
                        $floattable = $("<table class='tblResponsive'></table>");
                        $floattableheaderrow = $("<tr></tr>");
                        $floattableitemrow = $("<tr></tr>");
                        floattablecount = floattablecount + 1;
                        iTableColumns = 0;
                        iTableHeaders = 0;
                        bFloatLeftTableOpen = true;
                        bFloatHeadersOpen = false;

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatBlockLeft) {
                        //float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because FloatBlocks are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divEdit.append("<div id='div" + item.fieldTagID + "FloatBlock' style='float:left;'></div>");
                        divActive = $("#div" + item.fieldTagID + "FloatBlock");
                        domLastID = "div" + item.fieldTagID + "FloatBlock";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TableHeader) {
                        if (bFloatLeftTableOpen == true) {
                            bFloatHeadersOpen = true;
                            iTableHeaders = iTableHeaders + 1;
                            $floattableheaderrow.append("<th style='border:none;'>" + item.fieldCaption + "</th>");
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Label) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append("<td class='tblResponsive-top'>" + item.fieldCaption + "</td>");
                        } else {
                            //add a Responsive table with just a caption (no data) -- this should not be used
                            sFormated = kendo.format("<div id='div{0}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{1}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'></td></tr></table></div>", item.fieldTagID, item.fieldCaption);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox'  /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            //add a Responsive table
                            sFormated = kendo.format(" <div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox' class='k-checkbox' /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false && item.fieldMaxlength > 150) {

                        var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";

                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else {

                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' {4} /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input id='{3}' {4} /></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }
                    }
                    if (isEmpty(sFormated) === false){
                        if (item.fieldInsertOnly === true)
                        {
                            $InsertOnlyHTML.append(sFormated);
                        } else {                        
                            divActive.append(sFormated);
                        }
                    }
                }
                

            });
            if (bFloatLeftTableOpen == true) {
                //append the open float table to the active div and create a new float table
                //float tables are always nested inside a <div style="float: left;">
                divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>",floattablecount.toString()));
                $floattable.append($floattableitemrow);
                $("#divFloatTable-" + floattablecount.toString()).append($floattable);   
            }           
            //divEdit.append($InsertOnlyHTML) ;   
            divWrapper.append($HiddenFields);
            var sHT = $("#" + this.ContainerDivID).html();
        };
    };
     
    this.loadKendo = function () {
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var tObj = this;
            this.blnEditing = false; 
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                if (tObj.blnEditing === true && ( (item.fieldVisible === false && item.fieldReadOnly === true)  || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing no kindo widgets for these hidden where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  no kindo widgets for fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMobileSwitch");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoMobileSwitch();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoNumericTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ format: item.fieldFormat, decimals: 6 });
                        } else {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ decimals: 6 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDatePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDatePicker({ format: item.fieldFormat  });
                        } else {
                            $("#" + item.fieldTagID).kendoDatePicker();
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDateTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: item.fieldFormat, interval: 15 });
                        } else {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: "HH:mm ", interval: 15 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoTimePicker();
                        };
                    }                      
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoEditor");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoEditor({
                            resizable: {
                                content: false,
                                toolbar: false

                            },
                            // Empty tools so do not display toolbar
                            tools: []
                        });
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoColorPicker");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoColorPicker();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {                    
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDropDownList");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoDropDownList({
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        $.ajax({
                                            async: true,
                                            type: "GET",
                                            url: "api/" + item.fieldAPIReference + "/" + item.fieldAPIFilterID,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {
                                                options.success(data);
                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get " + item.fieldName + "  Failure");
                                                ngl.showErrMsg("Read " + item.fieldName + " Error", sMsg, null);
                                            }
                                        });
                                    }
                                },
                                schema: {
                                    data: "Data",
                                    total: "Count",
                                    model: {
                                        id: "Control",
                                        fields: {
                                            Name: { editable: false },
                                            Description: { editable: false }
                                        }
                                    },
                                    errors: "Errors"
                                },
                                error: function (e) {
                                    ngl.showErrMsg("Read " + item.fieldName + " Error", e.errors, null);
                                    this.cancelChanges();
                                },
                                serverPaging: false,
                                serverSorting: false,
                                serverFiltering: false
                            },
                            change:  function(e) { var tList = this; if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) { tObj.ListChanged(e,tList); }} ,
                            dataBound: function (e) {                              
                                var listContainer = e.sender.list.closest(".k-list-container");
                                var iNewWidth = listContainer.width() + 25;
                                listContainer.width(iNewWidth);
                                var tList = this;
                                if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) {
                                    tObj.ListChanged(e, tList);
                                }
                            },
                        });
                        var localDropDown = $("#" + item.fieldTagID).data("kendoDropDownList");
                        localDropDown.list.width("auto");
                    } else {
                        nglkendoitem.dataSource.read();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMaskedTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoMaskedTextBox({ mask: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoMaskedTextBox();
                        };
                    }

                };               
            });
        };
    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        //debugger;
        var tObj = this;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }
        
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            //debugger;
            if (typeof (errWarnData) !== 'undefined') { errWarnData = data; }
            //console.log(errwarnData);
            //if (typeof (wdgtNGLErrWarnMsgLogCtrlDialog) !== 'undefined' && wdgtNGLErrWarnMsgLogCtrlDialog != null ) {
            //    wdgtNGLErrWarnMsgLogCtrlDialog.show(data);
            //}            
        }
        this.rData = null;
        try {
            //var windowWidget = $("#" + this.ContainerDivID)
            //kendo.ui.progress($("#" + this.ContainerDivID), false);
            kendo.ui.progress($("#" + this.ContainerDivID), false);
            //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.",null)
        }
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    this.raiseAction("ReadFailure");
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) ) {
                        blnSuccess = true;
                        oResults.datatype = "object";
                        oResults.data = data.Data[0];
                        oResults.msg = "Success"
                        var blnShowAlert = false;
                        var sAlertMsg = "";
                        var sTitle = "";
                        if (typeof (data.Warnings) !== 'undefined' && data.Warnings != null && data.Warnings.length > 0) {
                            blnShowAlert = true;
                            sTitle = "Warnings"
                            sAlertMsg = data.Warnings;
                        }
                        if (typeof (data.Messages) !== 'undefined' && data.Messages != null && data.Messages.length > 0) {
                            if (blnShowAlert == true) {
                                sTitle = "Warnings and Messages";
                            } else {
                                blnShowAlert = true;
                                sTitle = "Messages";
                            }                           
                            sAlertMsg += data.Messages;
                        }
                        if (blnShowAlert == true) {
                            ngl.Alert(sTitle, sAlertMsg, 400, 400)
                            oResults.Dialog = ngl.AlertDialog;
                        } else {
                            ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);
                        }                        
                        this.raiseAction("Saved");
                        //Only close the window if the save was successful
                        //this.kendoWindowsObj.close();
                            
                        //ngl.showSuccessMsg(oResults.msg, tObj);
                    } else {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Unable to save your " + this.DataSourceName + " changes";
                        oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                        this.raiseAction("SaveError");
                    }

                    try{
                        if (typeof (tObj.ParentIDKey) !== 'undefined' && tObj.ParentIDKey !== null){
                            var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");
                            if (typeof (grid) !== 'undefined' && grid !== null) {
                                if (tObj.blnEditing == false) {
                                    grid.dataSource.page(1);
                                }
                                grid.dataSource.read();
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.message, null);
                    }
                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save " + this.DataSourceName + " Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                this.raiseAction("SaveError");
            }
        } catch (err) {
            oResults.error = err;
            ngl.showErrMsg(err.name, err.message, null);
            this.raiseAction("SaveError");
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        $("#div" + this.IDKey + "wait").hide();
    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        //debugger;
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    var windowWidget = $("#" + this.ContainerDivID).data("kendoWindow");
        //    kendo.ui.progress(windowWidget.element, false);
        //    //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        //try{
        //    kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.",null)
        //}
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed 
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        this.raiseAction("SaveFailure");
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        $("#div" + this.IDKey + "wait").hide();
    }
    
    this.readSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read"
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    this.raiseAction("ReadFailure");
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = this.DataType;
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        this.raiseAction("Read");                       
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        this.raiseAction("ReadError");
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
                this.raiseAction("ReadError");
            }
        } catch (err) {
            oResults.error = err
            ngl.showErrMsg(err.name, err.message, null);
            this.raiseAction("ReadError");
        }
        this.show(this.readKey);
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }


    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) { 
       
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        this.raiseAction("ReadError");
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }
    
    this.deleteSuccessCallback = function (data) {
        return ""; //not currently supported

        //var oResults = new nglEventParameters();
        //var tObj = this;
        //oResults.source = "deleteSuccessCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed  
        //oResults.CRUD = "delete"
        //var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        //if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
        //    kendo.ui.progress(parentGrid.element, false);
        //}
        ////clear any old return data in rData
        //this.rData = null;
        //try {            
        //    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
        //        if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
        //            oResults.error = new Error();
        //            oResults.error.name = "Delete " + this.DataSourceName + " Failure";
        //            oResults.error.message = data.Errors;
        //            ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
        //        } else {
        //            if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
        //                oResults.datatype = "bool";
        //                oResults.data = data.Data[0];
        //                oResults.msg = "Success"
        //                ngl.showSuccessMsg("Success your " + this.DataSourceName + " record has been deleted", tObj);
        //            }
        //            else {
        //                oResults.error = new Error();
        //                oResults.error.name = "Unable to delete your " + this.DataSourceName + " record";
        //                oResults.error.message = "The delete procedure returned false, please refresh your data and try again.";
        //                ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
        //            }
                    
        //            try {
        //                if (typeof (tObj.ParentIDKey) !== 'undefined' &&  tObj.ParentIDKey !== null) {
        //                    var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");
        //                    grid.dataSource.page(1);
        //                    grid.dataSource.read();
        //                }
        //            } catch (err) {
        //                ngl.showErrMsg(err.name, err.message, null);

        //            }
        //        }
        //    } else {
        //        oResults.msg = "Success but no data was returned. Please refresh your page and check the results.";
        //        ngl.showSuccessMsg(oResults.msg, null);
        //    }
        //} catch (err) {
        //    oResults.error = err
        //    ngl.showErrMsg(err.name, err.message, null);
        //}
        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}

    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported
        //var oResults = new nglEventParameters();
        //var tObj = this;
        //var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        //if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
        //    kendo.ui.progress(parentGrid.element, false);
        //}
        //oResults.source = "deleteAjaxErrorCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed
        //oResults.CRUD = "delete"
        //oResults.error = new Error();
        //oResults.error.name = "Delete " + this.DataSourceName + " Failure"
        //oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        var oRet = new validationResults();
        var tObj = this;
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save " + this.DataSourceName  + " Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save " + this.DataSourceName  + " Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) { 
            this.blnEditing = false;    
            if (typeof (editing) === 'undefined') { this.blnEditing = false;} else { this.blnEditing = editing == "true";}
            
            $.each(this.dataFields, function (index, item) {
                if (item.fieldName in data)
                {
                    //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                    if (
                        (tObj.blnEditing === true && ( (item.fieldVisible === false && item.fieldReadOnly === true)  || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                        ||
                        (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) 
                    ) {
                        //valid 
                    }  else {
                        if (item.fieldRequired === true ){ 
                            var field = data[item.fieldName];                       
                            if (isEmpty(field) === true) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                        }
                    }
                }
                //if (tObj.blnEditing === true) { //update changes
                //    if (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)) {
                //        //valid
                //    } else  if ((item.fieldAllowNull === false || item.fieldRequired === true)  && (item.fieldName in data)  ) {
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //} else {
                //    if (item.fieldVisible === false  && item.fieldInsertOnly === false && item.fieldRequired === false ) {
                //        //valid
                //    } else if ((item.fieldAllowNull === false || item.fieldRequired === true )  && (item.fieldName in data)  )  { 
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //}

            });
        }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }
       
    // Generic CRUD functions for all Widgets
    this.save = function () {
        //debugger;
        //kendo.ui.progress($(document.body), true);

        //kendo.ui.progress($("#" + this.ContainerDivID).kendoWindow.element, true);

        //var windowWidget = $("#" + this.ContainerDivID).data("kendoWindow");

        
        //kendo.ui.progress(windowWidget.element, true);
        
        //this.kendoWindowsObj = $("#" + this.ContainerDivID).kendoWindow
        //$("#div" + this.IDKey + "wait").show();

        var tObj = this;
        var windowWidget = $("#" + this.ContainerDivID).data("kendoWindow");
        kendo.ui.progress(windowWidget.element, true);
        setTimeout(function (tObj) {
            //first we save all of the container data
            if( tObj.ctrlContainers != null){
                $.each(tObj.ctrlContainers, function (index, item) {
                    //currently only some objects support saving
                    if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                        var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                        if (typeof (tPage[sControlVariableName]) !== 'undefined'  && ngl.isObject(tPage[sControlVariableName]) ){
                            tPage[sControlVariableName].save();
                        }
                    } 
                });
            }
            if (typeof (tObj.dataFields) !== 'undefined' && ngl.isObject(tObj.dataFields) === true && ngl.isArray(tObj.dataFields) === true) {
                tObj.blnEditing = false;
                //if we have data we have an existing record to edit so set blnEditing to true
                if (typeof (tObj.data) !== 'undefined' && ngl.isObject(tObj.data)) {
                    tObj.blnEditing = true;
                };
                tObj.data = new window[tObj.DataType]; //clear any old data
                //read the fields
                $.each(tObj.dataFields, function (index, item){
                    var field = tObj.data[item.fieldName];
                    var elmt = $("#" + item.fieldTagID);                
                    if (typeof (elmt) !== 'undefined' && elmt != null) {
                        try {
                            if (
                                (tObj.blnEditing === true && ( (item.fieldVisible === false && item.fieldReadOnly === true)  || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                                ||
                                (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) 
                            ){ 
                                //hidden fields                                
                                var hVal = elmt.val();
                                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                    field = ngl.nbrTryParse(hVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                                } else {
                                    field = hVal;
                                };
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                                field = elmt.prop("checked");
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                                field = elmt.data("kendoMobileSwitch").check();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                var numVal = elmt.data("kendoNumericTextBox").value();
                                field = ngl.nbrTryParse(numVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                field = elmt.data("kendoDatePicker").value();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                field = elmt.data("kendoDateTimePicker").value();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                field = ngl.convertTimePickerToDateString(elmt.data("kendoTimePicker").value(), null, null);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {  
                                field = elmt.data("kendoEditor").value();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                field = elmt.data("kendoColorPicker").value();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                field = elmt.data('kendoDropDownList').value();
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                                field = elmt.data("kendoMaskedTextBox").value();
                            };
                        } catch (err) {
                            //just read the jquery value
                            var sVal = elmt.val();
                            if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                field = ngl.nbrTryParse(sVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                            } else {
                                field = sVal;
                            };
                        }
                    } else {
                        field = item.fieldDefaultValue;
                    }
                    tObj.data[item.fieldName] = field;
                });  
                var oValidationResults = tObj.validateRequiredFields(tObj.data,tObj.blnEditing);
                if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                    ngl.showErrMsg("Cannot validate " + tObj.DataSourceName + " Information", "Invalid validation procedure, please contact technical support", tObj);
                    return;
                }  else { 
                    if (oValidationResults.Success === false) { 
                        var wdth = ($(window).width() / 10) * 6;
                        var hgt = ($(window).height() / 10) * 6;
                        ngl.Alert("Required Fields",oValidationResults.Message, wdth, hgt);
                        return; 
                    } 
                }
     
                //save the changes
                //var windowWidget = tObj.kendoWindowsObj.data("kendoWindow");
                //kendo.ui.progress(windowWidget.element, true);
                //var windowWidget = $("#wnd" + tObj.IDKey + "Edit").data("kendoWindow");
           
                //kendo.ui.progress(tObj.kendoWindowsObj.element, true);

            
                if (ngl.isNullOrWhitespace(tObj.API) === false) {
                
                    //var windowWidget = $("#" + tObj.ContainerDivID).data("kendoWindow");
                    //kendo.ui.progress(windowWidget.element, true);
                    //kendo.ui.progress(tObj.kendoWindowsObj.element, true);
                    //kendo.ui.progress(tObj.kendoWindowsObj, true);
               
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        if (oCRUDCtrl.update(tObj.API + "/POST", tObj.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback") == false) {
                            // kendo.ui.progress(tObj.kendoWindowsObj.element, false);
                            //kendo.ui.progress(windowWidget.element, false);
                        }
                
                } else {
                    tObj.raiseAction("Saved");
                }

            };
        }, 100, tObj);
        
    }

    this.show = function (fk) {
        //debugger;
        var tObj = this;
        var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
        if (!nglkendoWindow) {

            try {
                this.loadHTML();
                this.loadKendo();
                if (typeof (this.dataFields) === 'undefined' || ngl.isObject(this.dataFields) === false || ngl.isArray(this.dataFields) === false) { return; }
                //try{
                //    var dialog = $("#wnd" + this.IDKey + "Edit").data("kendoWindow");
                //    dialog.destroy();
                //    this.kendoWindowsObjUploadEventAdded = 0;
                //} catch (err) {
                //    //oResults.error = err
                //}
                //this.kendoWindowsObj = kendo.ui.Window;
                var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
                if (!nglkendoWindow) {

                    this.kendoWindowsObj = $("#" + this.ContainerDivID).kendoWindow({
                        title:  this.DataSourceName,
                        modal: true,
                        visible: false,
                        height: '75%',
                        width: '75%',
                        actions: ["save", "Minimize", "Maximize", "Close"],
                        close: function (e) { tObj.close(e); },
                        //deactivate: function () {
                        //    this.destroy();
                        //}
                    }).data("kendoWindow");
                }

                //this.kendoWindowsObj = $("#div" + this.IDKey).kendoWindow({
                //    title: "Add " + this.DataSourceName,
                //    modal: true,
                //    visible: false,
                //    height: '75%',
                //    width: '75%',
                //    actions: ["save", "Minimize", "Maximize", "Close"],
                //    close: function (e) { tObj.close(e); },
                //    deactivate: function () {
                //        this.destroy();
                //    }
                //}).data("kendoWindow");

                //if (this.kendoWindowsObjUploadEventAdded === 0) {
                try {
                    $("#" + this.ContainerDivID).data("kendoWindow").wrapper.find(".k-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
                } catch (err) {
                    ngl.showInfoNotification("Save not available", "Could not load save method please reload the page to save", null);
                }


                //}
                //this.kendoWindowsObjUploadEventAdded = 1;

                //if this is an edit load the data to the window
                //if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {           
                //    this.kendoWindowsObj.title(this.DataSourceName);
                //}        
               
            } catch (err) {

                ngl.showErrMsg("Cannot show Window", err.message, null);

            }
            //var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
            //if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
            //    kendo.ui.progress(parentGrid.element, false);
            //}
        }
        this.edit(this.data, fk);
        //this.kendoWindowsObj.refresh();
        //if (this.EditError === false) { this.kendoWindowsObj.center().open(); }
        $("#" + this.ContainerDivID).data("kendoWindow").center().open()
        if (this.ctrlContainers != null) {
            $.each(this.ctrlContainers, function (index, item) {
                //currently only some objects support saving
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                    var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined' && ngl.isObject(tPage[sControlVariableName])) {
                        tPage[sControlVariableName].read(fk);
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.GridFastTab) {
                    //get a reference to the fast tab's grid using the CRUD Tag ID
                    var grid = $("#" + item.fieldCRUDTagID).data("kendoNGLGrid");
                    if (typeof (grid) !== 'undefined' && grid != null) {
                        grid.dataSource.read();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Grid) {
                    //get a reference to the fast tab's grid using the CRUD Tag ID
                    var grid = $("#" + item.fieldTagID).data("kendoNGLGrid");
                    if (typeof (grid) !== 'undefined' && grid != null) {
                        grid.dataSource.read();
                    }
                }
            });
        }
    }
    
    this.read = function (intControl) {
        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) === 'undefined' || intControl === null) {
            intControl = 0;
        }
        this.readKey = intControl;
        this.rData = null;
        this.data = null;
        //kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
        if (ngl.isNullOrWhitespace(tObj.API) === false) {
            //setTimeout(function (tObj) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            if (oCRUDCtrl.read(tObj.API + "/GET", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                blnRet = false;
                //kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
            }
            //}, 1, tObj);
        } else {
            this.show(this.readKey);
        }

        //moved to show by RHR on 11/21/2018
        //***********************************************************************************         
        //if( this.ctrlContainers != null){
        //    $.each(this.ctrlContainers, function (index, item) {
        //        //currently only some objects support saving
        //        if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
        //            var sControlVariableName = "wdgt" + item.fieldName + "Edit"
        //            if (typeof (tPage[sControlVariableName]) !== 'undefined'  && ngl.isObject(tPage[sControlVariableName]) ){
        //                tPage[sControlVariableName].read(intControl);
        //            }
        //        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.GridFastTab) {
        //            //get a reference to the fast tab's grid using the CRUD Tag ID
        //            var grid = $("#" + item.fieldCRUDTagID).data("kendoNGLGrid");                    
        //            if (typeof (grid) !== 'undefined'  && grid != null ){
        //                grid.dataSource.read();
        //            }
        //        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Grid) {
        //            //get a reference to the fast tab's grid using the CRUD Tag ID
        //            var grid = $("#" + item.fieldTagID).data("kendoNGLGrid");                    
        //            if (typeof (grid) !== 'undefined'  && grid != null ){
        //                grid.dataSource.read();
        //            }
        //        }
        //    });
        //}
        //***********************************************************************************  
       
        return blnRet;
    }

    //The sActionKey indicates which actions to execute
    //common keys are save and close other custom keys may be 
    //configured by each page.  Each cmPageDetail object may 
    //have an action associated with a key via the PageDetWidgetActionKey
    //and the PageDetWidgetAction. additionally each container/widget
    //may implement other logic to determine which actions to execute
    //for example the NGLWorkFlowOptionCtrl may only execute actions
    //when a switch or group is selected.
    this.executeActions = function (sActionKey) {
        //debugger;
        var blnRet = true;
        var tObj = this;
        if (sActionKey === 'save'){
            this.save();
        }
        if( this.ctrlContainers != null){
            $.each(this.ctrlContainers, function (index, item) {
                //currently only some objects support actions
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                    var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined'  && ngl.isObject(tPage[sControlVariableName]) ){
                        tPage[sControlVariableName].executeActions(sActionKey);
                    }
                } 
            });
        }
        
        if (sActionKey === 'close'){
            this.kendoWindowsObj.close();
        }
        //if (typeof (intControl) != 'undefined' && intControl != null) {
        //    this.rData = null;
        //    this.data = null;
        //    //save the changes
        //    //var windowWidget = this.kendoWindowsObj.data("kendoWindow");
        //    //kendo.ui.progress(windowWidget.element, true);
        //    //kendo.ui.progress(this.kendoWindowsObj.element, true);
        //if (ngl.isNullOrWhitespace(tObj.API) === false) {
        //    setTimeout(function (tObj) {
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        if (oCRUDCtrl.read(tObj.API + "/GET", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
        //            blnRet = false;
        //            //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        //            //kendo.ui.progress(windowWidget.element, false);
        //        }
        //    }, 2000, tObj);
        //}
        //}
        return blnRet;
    }

    this.raiseAction = function (sAction) {
        if (!sAction) { return;}
        if (isEmpty(sAction)) { return;}
        if (typeof (tPage.execActionClick) !== 'undefined' && ngl.isFunction(tPage.execActionClick)) {
            tPage.execActionClick(sAction,this.sNGLCtrlName);
        }
    }

    this.delete = function (intControl) {
        return ''; //not currently supported
        //var blnRet = true;
        //var tObj = this;
        //if (typeof (intControl) != 'undefined' && intControl != null) {
        //    this.rData = null;
        //    this.data = null;
         
        //    var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        //    if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
        //        kendo.ui.progress(parentGrid.element, true);
        //    }
        //if (ngl.isNullOrWhitespace(tObj.API) === false) {
        //    setTimeout(function (tObj) {
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        if (oCRUDCtrl.delete(tObj.API + "/DELETE", intControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback") == false) {
        //            blnRet = false;
        //            var parentGrid = $('#' + tObj.ParentIDKey).data("kendoNGLGrid");
        //            if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
        //                kendo.ui.progress(parentGrid.element, false);
        //            }
        //            //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        //            //kendo.ui.progress(windowWidget.element, false);
        //        }
        //    }, 2000, tObj);
        //}
        //}
        //return blnRet;
    }

    this.edit = function (data,fk) {
        //return ""; //not currently supported
        //load data to the screen
        var tObj = this; 
        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            if ($('#div' + this.IDKey + 'InsertOnly')) { $('#div' + this.IDKey + 'InsertOnly').hide(); }
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldName in data) {
                        var blnReadOnly = false;
                        if (item.fieldReadOnly === true ) { blnReadOnly = true; }
                        var dataItem = data[item.fieldName];
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if ((item.fieldVisible === false && item.fieldReadOnly === true)  || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))) {
                                //no kindo widgets for hidden fields  
                                $("#" + item.fieldTagID).val(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {                                
                                if (dataItem !== true) {dataItem = false;}
                                $("#" + item.fieldTagID).prop('checked', dataItem);                                
                                $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                                if (dataItem !== true) {dataItem = false;}
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").check(dataItem );
                                //$("#" + item.fieldTagID).data("kendoMobileSwitch").value({ checked: dataItem });
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").enable(!blnReadOnly );
                                //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoMobileSwitch").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                $("#" + item.fieldTagID).data("kendoDatePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                $("#" + item.fieldTagID).data("kendoTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                                $("#" + item.fieldTagID).data("kendoEditor").value(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                                $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                $("#" + item.fieldTagID).data("kendoDropDownList").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                            };
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.EditErrorTitle, this.EditErrorMsg, null); this.EditError = true; return false;
            };

        }   else {
            if ($('#div' + this.IDKey + 'InsertOnly')) { $('#div' + this.IDKey + 'InsertOnly').show(); }
            //we are adding a new record
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    var blnReadOnly = false;
                    if (item.fieldInsertOnly === false && item.fieldReadOnly === true && item.fieldRequired === false) {blnReadOnly = true;}
                    var blnHasDefaultVal = false;
                    if (typeof (item.fieldAPIReference) !== 'undefined' &&  item.fieldAPIReference !== null && item.fieldAPIReference == "fk" && typeof (fk) !== 'undefined' && fk !== null) {
                        blnHasDefaultVal = true;
                        item.fieldDefaultValue = fk;
                    } else {                   
                        
                        if (typeof (item.fieldDefaultValue) !== 'undefined' && item.fieldDefaultValue !== null && isEmpty(item.fieldDefaultValue) === false) { blnHasDefaultVal = true; }
                    }
                    if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)) {
                        //no kindo widgets for hidden fields  
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).val(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).val();
                        };
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue !== true) {item.fieldDefaultValue = false;}
                            $("#" + item.fieldTagID).prop('checked', item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).prop('checked', false);
                        }
                        $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue !== true) {item.fieldDefaultValue = false;}
                            $("#" + item.fieldTagID).data("kendoMobileSwitch").value({ checked: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoMobileSwitch").value({ checked: false });
                        }
                        $("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);                     
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly); 
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly); 
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoEditor").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoEditor").value();
                        }                      
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly); 
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDropDownList").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoDropDownList").select(0);
                        }                        
                        $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                    } else {
                        if ($("#" + item.fieldTagID).data("kendoMaskedTextBox")) {
                            if (blnHasDefaultVal === true) {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(item.fieldDefaultValue);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value();
                            }
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                            //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                        };
                        
                    };
                });
            } else {
                ngl.showValidationMsg(this.AddErrorTitle, this.AddErrorMsg, null); this.EditError = true; return false;
            };
        };
        return true;
    }
        
    this.close = function (e) {        
        $("#div" + this.IDKey + "wait").hide();
    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID, 
                                    WindowPageVariable, 
                                    IDKey, 
                                    ParentIDKey,
                                    fieldData, 
                                    Theme, 
                                    CallBackFunction, 
                                    API, 
                                    DataType,
                                    DataSourceName,
                                    EditErrorMsg,
                                    EditErrorTitle,
                                    AddErrorMsg,
                                    AddErrorTitle,
                                    sNGLCtrlName) {

        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.ParentIDKey = ParentIDKey;
        this.Theme = Theme;
        this.kendoWindowsObj = WindowPageVariable;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;
        this.EditErrorMsg = EditErrorMsg;
        this.EditErrorTitle = EditErrorTitle;
        this.AddErrorMsg = AddErrorMsg;
        this.AddErrorTitle = AddErrorTitle;
        this.sNGLCtrlName  = sNGLCtrlName;
        

    }

}

//NGLWorkFlowOptionCtrls (58) contain a group of switches that are turned on or off 
// switches can be listed individually with a parentId of the NGLWorkFlowOptionCtrl
//or they can be grouped together with a parentid of a NGLWorkFlowGroup element (59)
//when an individual switch is true or when any one of many grouped switches are 
//true all other switches are set to false and hidden.  you cannot nest NGLWorkFlowGroups 
//inside other NGLWorkFlowGroup
// if an NGLWorkFlowSectionCtrl is the child of an individual switch or a NGLWorkFlowGroup 
// it will be visible when that group is active (true) or when the associated switch is true.
// if the group or the switch evaluates to false the NGLWorkFlowSectionCtrl is hidden
// as switches are set to true or false the associated DataFieldDetails object is added to 
// a result list and 
// event trigger example:  http://api.jquery.com/trigger/
// a fast tab is generated automatically for this control so 
// one does not need to be declared. The caption for control will be
// use for the the fast tab caption
// this control does not support CRD only U so Insert Only and read only settings are ignored
// read opens the users previous settings stored with the page settings 
// for the control name;  The following data represents the settings 
// for the control with a key of the control name. a WorkFlowSetting object is defined in NGLobjects.js
// JSON Data Example: [{fieldID: 1,fieldName: "",fieldDefaultValue: "false",fieldVisible: "false",fieldReadOnly: "true" },{fieldID: 2,fieldName: "",fieldDefaultValue: "true",fieldVisible: "true",fieldReadOnly: "false"}]
// settings are read then saved everytime one of the NGLWorkFlow.Switch setting is modified
// The workflowsettings are returned to the call back method in the data object with a results source 
// of saveSuccessCallback or readSuccessCallback. The page should handle the call back and
//  store the workFlowSettings as a page level variable with a name like 
//  oWorkFlowSettings. The save method or next button on the container (page or popup winow) will
//  need this data to determine the next operation in the workflow.
function NGLWorkFlowOptionCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    //Widget specific properties
    this.DataSourceName = "Record";  //DataSourceName comes from the Caption property of the Widget.
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    screenLEName: null;
    IDKey: "id1234";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.PKName = "";
    this.sNGLCtrlName = ""; //varialbe used on page for the control like "wdgt" +  sCleanPageDetName + "Edit"
    this.switchIDs = null;
    this.workFlowSections = null;
    this.workFlowSettings = null;  //current array of switch options stored in WorkFlowSetting object
    this.SaveUserSettings = true;
    this.ReadUserSettings = true;
    //Widget specific functions
 
    this.ListChanged = function (e, tList) {
        var source = this;
        //NGLEDITOnPageListChanged
        if (typeof (NGLWorkFlowOptionListChanged) !== 'undefined' && ngl.isFunction(NGLWorkFlowOptionListChanged)) {
            NGLWorkFlowOptionListChanged(e, tList, source);
        }
    };
        
    this.onSwitchChange = function (e) {

        var tObj = this;
        var sID = e.sender.element[0].id
        if (!sID) { return; }
        if (!tObj.switchIDs) { return; }
        if (!tObj.dataFields) { return; }
        var oSwitchItem = tObj.GetFieldItem(sID);
        if (!oSwitchItem){return;}
        this.workFlowSettings = [];
        
        if (e.checked === true) {                    
            tObj.workFlowSettings.push({ fieldID: oSwitchItem.fieldID, fieldName: oSwitchItem.fieldName, fieldDefaultValue: 'true', fieldVisible: 'true', fieldReadOnly: "false" })
            if (oSwitchItem.fieldParentTagID == this.IDKey) {
                //hide all items with a different sID only one item can be selected at the root of the WorkFlowOptionCtrl
                $.each(tObj.dataFields, function (index, nitem) {
                    if (nitem.fieldTagID != sID){
                        if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                            ||
                            nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch){
                            $("#" + nitem.fieldTagID).data("kendoMobileSwitch").check(false);
                            $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(false);
                            $("#li" + nitem.fieldTagID).hide();
                            tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: 'false',fieldVisible: 'false',fieldReadOnly: "true"})
                        }else if(nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                            $("#li" + nitem.fieldTagID).hide();
                            tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'false',fieldReadOnly: "true"})
                        }else if(nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                            if (nitem.fieldParentTagID == sID){
                                $("#div" + nitem.fieldTagID + "wrapper").show();
                                tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'true',fieldReadOnly: "false"})
                            } else {
                                $("#div" + nitem.fieldTagID + "wrapper").hide();
                                tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'false',fieldReadOnly: "true"})
                            }                                    
                        }
                    }
                });
            } else {  //the switch is checked and it is part of a work flow group
                var sParentTagID = oSwitchItem.fieldParentTagID;
                var oParentItem = tObj.GetFieldItem(sParentTagID);
                if (!oParentItem) { return; }
                if (oParentItem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                    tObj.workFlowSettings.push({ fieldID: oParentItem.fieldID, fieldName: oParentItem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                    //hide all items not in the group and show all the items at or below the group
                    $.each(tObj.dataFields, function (index, nitem) {
                        if (nitem.fieldTagID !== sID) {
                            if (nitem.fieldParentTagID == sParentTagID) {
                                //same parent/group so show; the default on switches is true if checked
                                if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                                    ||
                                    nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                                    $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(true);
                                    $("#li" + nitem.fieldTagID).show();
                                    var sDefault = 'false';
                                    if ($("#" + nitem.fieldTagID).data("kendoMobileSwitch").check() == true) { sDefault = 'true'; }
                                    tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: sDefault, fieldVisible: 'true', fieldReadOnly: "false" })
                                } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                                    $("#div" + nitem.fieldTagID + "wrapper").show();
                                    tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                                }
                            } else {
                                //different parent/group so hide; the default on switchs is always false
                                if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                                    ||
                                    nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                                    $("#" + nitem.fieldTagID).data("kendoMobileSwitch").check(false);
                                    $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(false);
                                    $("#li" + nitem.fieldTagID).hide();
                                    tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: 'false', fieldVisible: 'false', fieldReadOnly: "true" })
                                } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                                    //hide all groups except this one
                                    if (nitem.fieldTagID != sParentTagID) {
                                        $("#li" + nitem.fieldTagID).hide();
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'false', fieldReadOnly: "true" })
                                    }
                                } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                                    if (nitem.fieldParentTagID == sID) {
                                        //this section is a child of the switch so show it
                                        $("#div" + nitem.fieldTagID + "wrapper").show();
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                                    } else {
                                        $("#div" + nitem.fieldTagID + "wrapper").hide();
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'false', fieldReadOnly: "true" })
                                    }
                                }
                            }
                        }
                    });
                }
            } // option switches are only allowd at the header or in a group so thete is no else option here                   
        } else { //not checked
            //the selected item is not checked so set the default value to false but it is also visible
            tObj.workFlowSettings.push({fieldID: oSwitchItem.fieldID, fieldName: oSwitchItem.fieldName,fieldDefaultValue: 'false',fieldVisible: 'true',fieldReadOnly: "false"})
            if (oSwitchItem.fieldParentTagID == this.IDKey)  { // the unchecked switch is at the header/root level
                //when any item at the header (root) level is unchecked we re-show all check boxes and groups
                //but we hide any sections for this item
                $.each(tObj.dataFields, function (index, nitem) {
                    if (nitem.fieldTagID !== sID) {
                        //this item is not the selected item
                        if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                            ||
                            nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                            //this item is a switch
                            if (typeof (nitem.fieldLockVisibility) !== 'undefined' && nitem.fieldLockVisibility.toString() === "true") {
                                //visibility is locked so we do not allow changes trust that the current settings are correct (generally this is off/hidden)
                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: nitem.fieldDefaultValue, fieldVisible: nitem.fieldVisible, fieldReadOnly: nitem.fieldReadOnly });
                            } else {
                                $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(true);
                                $("#li" + nitem.fieldTagID).show();
                                var sDefault = 'false';
                                if ($("#" + nitem.fieldTagID).data("kendoMobileSwitch").check() == true) { sDefault = 'true'; }
                                //default value represent checked or unchecked
                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: sDefault, fieldVisible: 'true', fieldReadOnly: "false" });
                            }                           
                            
                        } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                            if (typeof (nitem.fieldLockVisibility) !== 'undefined' && nitem.fieldLockVisibility.toString() === "true") {
                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: nitem.fieldVisible, fieldReadOnly: nitem.fieldReadOnly });
                            } else {
                                $("#li" + nitem.fieldTagID).show();
                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                            }
                        }else if(nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                            if (nitem.fieldParentTagID == sID){
                                $("#div" + nitem.fieldTagID + "wrapper").hide();
                                tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'false',fieldReadOnly: "true"})
                            } else {
                                if ( $("#div" + nitem.fieldTagID + "wrapper").is(':visible')){
                                    tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'true',fieldReadOnly: "false"})
                                } else {
                                    tObj.workFlowSettings.push({fieldID: nitem.fieldID, fieldName: nitem.fieldName,fieldDefaultValue: '',fieldVisible: 'false',fieldReadOnly: "true"})
                                }
                            }                             
                        }
                    }
                });
            } else { // the unchecked switch is part of a workflow group
                var sParentTagID = oSwitchItem.fieldParentTagID;
                var oParentItem = tObj.GetFieldItem(sParentTagID);
                if (!oParentItem) { return; }
                if (oParentItem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                    tObj.workFlowSettings.push({ fieldID: oParentItem.fieldID, fieldName: oParentItem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                    //check if any other switch in this group are checked
                    var blnIsOneGroupItemChecked = false;
                    $.each(tObj.dataFields, function (index, nitem) {
                        if (nitem.fieldTagID !== sID
                            &&
                            nitem.fieldParentTagID == sParentTagID
                            &&
                            (
                                nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                                ||
                                nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch
                            )) {
                            if ($("#" + nitem.fieldTagID).data("kendoMobileSwitch").check() == true) {
                                blnIsOneGroupItemChecked = true;
                                return;
                            }
                        }
                    });
                    //hide or show all based on the group checked flag if one item is checked the group is checked
                    $.each(tObj.dataFields, function (index, nitem) {
                        if (nitem.fieldTagID !== sID) {
                            if (nitem.fieldParentTagID != sParentTagID) {
                                //this is not a member of the group; different parent.
                                if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                                    ||
                                    nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                                    if (blnIsOneGroupItemChecked == true) { // when one is checked all non-related switches are off
                                        $("#" + nitem.fieldTagID).data("kendoMobileSwitch").check(false);
                                        $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(false);
                                        $("#li" + nitem.fieldTagID).hide();
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: 'false', fieldVisible: 'false', fieldReadOnly: "true" })
                                    } else {
                                        //when the group is unchecked we show all other group items
                                        if (typeof (nitem.fieldLockVisibility) !== 'undefined' && nitem.fieldLockVisibility.toString() === "true") {
                                            tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: sDefault, fieldVisible: nitem.fieldVisible, fieldReadOnly: nitem.fieldReadOnly });
                                        } else {
                                            $("#" + nitem.fieldTagID).data("kendoMobileSwitch").enable(true);
                                            $("#li" + nitem.fieldTagID).show();
                                            var sDefault = 'false';
                                            if ($("#" + nitem.fieldTagID).data("kendoMobileSwitch").check() == true) { sDefault = 'true'; }
                                            tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: sDefault, fieldVisible: 'true', fieldReadOnly: "false" })
                                        }
                                    }
                                } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                                    if (nitem.fieldTagID == sParentTagID) {
                                        //this is the current container so just save the seetings as visible (always visivle if the switch is changed
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                                    } else {
                                        if (blnIsOneGroupItemChecked == true) {
                                            //hide all other groups
                                            $("#li" + nitem.fieldTagID).hide();
                                            tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'false', fieldReadOnly: "true" })
                                        } else {
                                            if (typeof (nitem.fieldLockVisibility) !== 'undefined' && nitem.fieldLockVisibility.toString() === "true") {
                                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: nitem.fieldVisible, fieldReadOnly: nitem.fieldReadOnly });
                                                //We could add logic here to loop through all checkboxes in this group and only show groups (header text) 
                                                //when none of the checkbox have the fieldLockVisibility === "true"
                                                //but for now the page must determine which option group has fieldLockVisibility set to true
                                            } else {
                                                // if nothing is checked show all other groups
                                                $("#li" + nitem.fieldTagID).show();
                                                tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                                            }
                                        }
                                    }                                   
                                } else if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                                    if (nitem.fieldParentTagID == sID) {
                                        //this section is a child of the switch so hide it
                                        $("#div" + nitem.fieldTagID + "wrapper").hide();
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'false', fieldReadOnly: "true" })
                                    } else {
                                        //we trust that the visible setting matches the current check/uncheck options
                                        //so just save the current setting
                                        if ($("#div" + nitem.fieldTagID + "wrapper").is(':visible')) {
                                            tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'true', fieldReadOnly: "false" })
                                        } else {
                                            tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: '', fieldVisible: 'false', fieldReadOnly: "true" })
                                        }
                                    }
                                }
                            } else {
                                //this is a member of the group  we only hide sections if all switches are false
                                if (nitem.fieldGroupSubType == nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl && blnIsOneGroupItemChecked == false) {
                                    $("#div" + nitem.fieldTagID + "wrapper").hide();
                                    tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: 'false', fieldVisible: 'false', fieldReadOnly: "true" })
                                } else {
                                    //we trust that the visible setting matches the current check/uncheck options
                                    //so just save the current setting
                                    if ($("#div" + nitem.fieldTagID + "wrapper").is(':visible')) {
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: 'true', fieldVisible: 'true', fieldReadOnly: "false" })
                                    } else {
                                        tObj.workFlowSettings.push({ fieldID: nitem.fieldID, fieldName: nitem.fieldName, fieldDefaultValue: 'false', fieldVisible: 'false', fieldReadOnly: "true" })
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }
        this.saveSettings();               
    }

    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.SetFieldDefault = function (sName,sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItem = function (sID){
        var sRet = null;
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItemByFieldID = function (sFieldID){
        var sRet = null;
        if (!sFieldID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldID == sFieldID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }
    
    this.loadHTML = function () {
        //debugger;
        var tObj = this;
        //var sHtml = kendo.format("<div id='div{0}Border' class='ngl-blueBorderFullPageWide' style='position: relative; min-width:250px;' ><div id='div{0}Edit' style='padding: 0px 10px 10px 10px;'><div style='margin:5px;'><span id='sp{0}Lbl' style='font-size:large; font-weight:bold'>{1}</span>&nbsp;&nbsp;<a onclick='{2}.save();' class='k-button' href='#'><span class='k-icon k-i-save'></span>Save</a></div>", this.IDKey,this.DataSourceName,this.sNGLCtrlName);


        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            tObj.blnEditing = true;
            
            var divContainer = $("#" + this.ContainerDivID);
            if (!divContainer) { return false; }
            var sHT1 = $("#" + this.ContainerDivID).html();
            //var divThisID = "div" + this.IDKey + "wrapper";
            //divContainer.append(kendo.format("<div id='{0}' style='position:relative;'></div>", divThisID));
            //add the fast tab
            var oOptionFastTab = new NGLFastTabCtrl();
            var sheaderID = "div" + this.IDKey + "Options";
            var sSectionID = "div" + this.IDKey + "Selctions"
            oOptionFastTab.appendToContainer(this.IDKey, this.ContainerDivID, true, sheaderID, null, this.DataSourceName)
            //if (document.getElementById(sheaderID) == null) {
            //    var st = 'Options does not exits'
            //}
            //if (document.getElementById(sSectionID) == null) {
            //    var st = 'Selctions does not exits'
            //}
            if($("#" + sheaderID).length == 0) {
                 divContainer.append(kendo.format("<div id='{0}' class='RateITOptions' style='position:relative;'></div>", sheaderID));
            }
            
            if($("#" + sSectionID).length == 0) {
                divContainer.append(kendo.format("<div id='{0}' style='position:relative;'></div>", sSectionID));
            }
            var sHT1 = $("#" + this.ContainerDivID).html();
            var divEdit = $("#" + sheaderID);
            var divSections = $("#" + sSectionID);
            var divActive = divEdit;   
            var ulGroup = null;
            var sParentULID = "ul" + this.IDKey 
            // this control  is a ul list to add the ul container
            divActive.append(kendo.format("<ul id='{0}' ></ul>", sParentULID));
            var ulParent = $("#" + sParentULID);
            $.each(this.dataFields, function (index, item) {
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowGroup) {
                    ulParent.append(kendo.format("<li id='li{0}' style='display: normal;'>{1}<ul  id='ul{0}'></ul></li>", item.fieldTagID, item.fieldCaption));
                    var divThisitem = $("#li" + item.fieldTagID);
                    if (divThisitem) {
                        if (item.fieldVisible.toString() !== 'true') {
                            divThisitem.hide();
                        } else {
                            divThisitem.show();
                        }
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch) {
                    // the parent of all NGLWorkFlowOnOffSwitch items must be a ul assigned the parent tag id 
                    var divThisParentUL = $("#ul" + item.fieldParentTagID);
                    if (divThisParentUL) {
                        divThisParentUL.append(kendo.format("<li id='li{0}' style='display: normal;'>{1}<input type='checkbox' id='{0}' aria-label='{1}' /></li>", item.fieldTagID, item.fieldCaption));
                        //add the switch to the array
                        if (tObj.switchIDs == null) {
                            tObj.switchIDs = new Array();
                        }
                        tObj.switchIDs.push(item);
                    }
                    var divThisitem = $("#li" + item.fieldTagID);
                    if (divThisitem) {
                        if (item.fieldVisible.toString() !== 'true') {
                            divThisitem.hide();
                        } else {
                            divThisitem.show();
                        }
                    }
                        
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                    // the parent of all NGLWorkFlowYesNoSwitch items must be a ul assigned the parent tag id 
                    var divThisParentUL = $("#ul" + item.fieldParentTagID);
                    if (divThisParentUL) {
                        divThisParentUL.append(kendo.format("<li id='li{0}' style='display: normal;'>{1}<input type='checkbox' id='{0}' aria-label='{1}' /></li>", item.fieldTagID, item.fieldCaption));
                        //add the switch to the array
                        if (tObj.switchIDs == null) {
                            tObj.switchIDs = new Array();
                        }
                        tObj.switchIDs.push(item);
                    }
                    var divThisitem = $("#li" + item.fieldTagID );
                    if (divThisitem) {                        
                        if (item.fieldVisible.toString() !== 'true') {
                            divThisitem.hide();
                        } else {
                            divThisitem.show();
                        }
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                    //add the switch to the array
                    if (tObj.workFlowSections == null) {
                        tObj.workFlowSections = new Array();
                    }
                    tObj.workFlowSections.push(item);
                    //move the section wrapper to the correct position on the container
                    var sWorkFlowSectionID = "div" + item.fieldTagID + "wrapper"
                    var divThisSection = $("#" + sWorkFlowSectionID);
                    if ($("#" + sWorkFlowSectionID).length == 0) {                   
                        divSections.append(kendo.format("<div id='{0}' style='position:relative;'></div>", sWorkFlowSectionID));
                        divThisSection = $("#" + sWorkFlowSectionID);
                    } else {
                        divSections.append(divThisSection);
                    }
                    
                    if (divThisSection) {                        
                        if (item.fieldVisible.toString() !== 'true') {
                            divThisSection.hide();

                        } else {
                            divThisSection.show();
                        }
                    }
                   
                }

            });
            if (oOptionFastTab) {
                if (oOptionFastTab.Expanded === true) {
                    oOptionFastTab.Expand();
                } else {
                    oOptionFastTab.Collapse();
                }
            }
            divContainer.append( kendo.format(" <br id='br{0}' style='clear:both; display: block; float: none;' />", tObj.IDKey));
            
            //code to help debug html issues
            var sHT = $("#" + this.ContainerDivID).html();
        };


    };
    
    this.loadKendo = function () {
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var tObj = this;
            this.blnEditing = true;
            $.each(this.dataFields, function (index, item) {
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMobileSwitch");
                    if (!nglkendoitem) {
                        var blnSwichChecked = false;
                        if (item.fieldDefaultValue === 'true') {blnSwichChecked = true;}
                        $("#" + item.fieldTagID).kendoMobileSwitch({
                            onLabel: "YES",
                            offLabel: "NO",
                            change: function (e) { tObj.onSwitchChange(e); },
                            checked: blnSwichChecked

                        });                       
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMobileSwitch");
                    if (!nglkendoitem) {
                        var blnSwichChecked = false;
                        if (item.fieldDefaultValue === 'true') {blnSwichChecked = true;}
                        $("#" + item.fieldTagID).kendoMobileSwitch({
                            change: function (e) { tObj.onSwitchChange(e); },
                            checked: blnSwichChecked
                        });                       
                    }
                } 
            });
        };
    };

    this.loadDeaultWorkFlows = function () {
        var tObj = this;
        this.workFlowSettings = [];
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
           
            $.each(this.dataFields, function (index, item) {
                
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowGroup
                    || item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOnOffSwitch
                    || item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowYesNoSwitch) {
                    tObj.workFlowSettings.push({ fieldID: item.fieldID, fieldName: item.fieldName, fieldDefaultValue: item.fieldDefaultValue, fieldVisible: item.fieldVisible, fieldReadOnly: item.fieldReadOnly, fieldLockVisibility: "false"});
                        
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                    tObj.workFlowSettings.push({ fieldID: item.fieldID, fieldName: item.fieldName, fieldDefaultValue: item.fieldDefaultValue, fieldVisible: "false", fieldReadOnly: "false", fieldLockVisibility: "false" });
                }

            });
        };

    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        return ""; //not currently supported

        //var oResults = new nglEventParameters();
        //oResults.source = "saveSuccessCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed   
        //oResults.CRUD = "update"
        //this.rData = null;
        //var tObj = this;        
        //try {
        //    var blnSuccess = false;
        //    var blnErrorShown = false;
        //    var strValidationMsg = "";
        //    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
        //        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
        //            oResults.error = new Error();
        //            oResults.error.name = "Save " + this.DataSourceName + " Settings Failure";
        //            oResults.error.message = data.Errors;
        //            blnErrorShown = true;
        //            //ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
        //        }
        //        else {
        //            this.rData = data.Data;
        //            if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
        //                blnSuccess = true;
        //                oResults.datatype = "WorkFlowSetting";
        //                oResults.data = this.workFlowSettings;
        //                oResults.msg = "Success"
        //                //ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);
        //            } else {
        //                blnErrorShown = true;
        //                oResults.error = new Error();
        //                oResults.error.name = "Unable to save your " + this.DataSourceName + " settings";
        //                oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
        //                //ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
        //            }

        //        }
        //    }
        //    if (blnSuccess === false && blnErrorShown === false) {
        //        oResults.error = new Error();
        //        oResults.error.name = "Save " + this.DataSourceName + " Settings Failure";
        //        oResults.error.message = "No results are available.";
        //        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        //    }
        //} catch (err) {
        //    oResults.error = err;
        //    ngl.showErrMsg(err.name, err.message, null);

        //}
        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported

        //var oResults = new nglEventParameters();
        //var tObj = this;
        
        //oResults.source = "saveAjaxErrorCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed 
        //oResults.CRUD = "update"
        //oResults.error = new Error();
        //oResults.error.name = "Save " + this.DataSourceName + " Settings Failure"
        //oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}
    }

    this.saveSettingsSuccessCallback = function (data) {
        //debugger;
        var tObj = this;   
        var oResults = new nglEventParameters();
        oResults.source = "saveSettingsSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        oResults.CRUD = "update"
        this.rData = null;     
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Settings Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    //ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        blnSuccess = true;
                        oResults.datatype = "WorkFlowSetting";
                        oResults.data = this.workFlowSettings;
                        oResults.msg = "Success"
                        //ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);
                    } else {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Unable to save your " + this.DataSourceName + " settings";
                        oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                        //ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save " + this.DataSourceName + " Settings Failure";
                oResults.error.message = "No results are available.";
                //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err;
            ngl.showErrMsg(err.name, err.message, null);

        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.saveSettingsAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        
        oResults.source = "saveSettingsAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed 
        oResults.CRUD = "update"
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Settings Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.readSuccessCallback = function (data) {
        return ""; //not currently supported
        
        //var oResults = new nglEventParameters();
        //var tObj = this;
        //try {
        //    kendo.ui.progress($("#" + tObj.ContainerDivID ), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        //oResults.source = "readSuccessCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed         
        //oResults.CRUD = "read"
        ////clear any old return data in rData
        //this.rData = null;
        //try {
        //    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
        //        if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
        //            oResults.error = new Error();
        //            oResults.error.name = "Read " + this.DataSourceName + " Settings Failure";
        //            oResults.error.message = data.Errors;
        //            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        //        }
        //        else {
        //            this.rData = data.Data;
        //            if (data.Data != null) {                       
        //                oResults.datatype = "WorkFlowSetting";
        //                oResults.msg = "Success"
        //                this.workFlowSettings = JSON.parse(data.Data[0].value); 
        //                oResults.data = this.workFlowSettings
        //                this.show();
        //            }
        //            else {
        //                oResults.error = new Error();
        //                oResults.error.name = "Invalid Request";
        //                oResults.error.message = "No Data available.";
        //                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        //            }
        //        }
        //    } else {
        //        oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
        //        ngl.showSuccessMsg(oResults.msg, tObj);
        //    }
        //} catch (err) {
        //    oResults.error = err
        //}
        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported

        //var oResults = new nglEventParameters();
        //var tObj = this;
        //try {
        //    kendo.ui.progress($("#" + tObj.ContainerDivID ), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        //oResults.source = "readAjaxErrorCallback";
        //oResults.widget = this;
        //oResults.msg = 'Failed'; //set default to Failed        
        //oResults.CRUD = "read"
        //oResults.error = new Error();
        //oResults.error.name = "Read " + this.DataSourceName + " Settings Failure"
        //oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        //if (ngl.isFunction(tPage[this.callback])) {
        //    tPage[this.callback](oResults);
        //}
    }
    
    //for now this is the only read method.  If data other than
    // user settings is required the read method would call readSuccessCallback 
    //which would then call readUserSettings
    this.readUserSettingsSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#" + tObj.ContainerDivID ), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        oResults.source = "readUserSettingsSuccessCallback";     
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read";
        oResults.widget = tObj;
        var blnDeleteUserSettings = false;
        
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    if (this.ReadUserSettings === true) {
                        oResults.error = new Error();
                        oResults.error.name = "Read " + this.DataSourceName + " Settings Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    } else {
                        blnDeleteUserSettings = true;
                    }
                    
                }
                else {
                    if (this.ReadUserSettings === true) {
                        this.rData = data.Data;
                        if (data.Data != null) {                       
                            oResults.datatype = "WorkFlowSetting";
                            oResults.msg = "Success"
                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data) && ngl.isObject(data.Data[0]) &&  data.Data[0].value !== null) {
                                this.workFlowSettings = JSON.parse(data.Data[0].value);
                                oResults.data = this.workFlowSettings
                            }                             
                        }
                        else {
                            oResults.error = new Error();
                            oResults.error.name = "Invalid Request";
                            oResults.error.message = "No Data available.";
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                    } else {
                        blnDeleteUserSettings = true;
                        oResults.datatype = "WorkFlowSetting";
                        oResults.msg = "Success"
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (this.ReadUserSettings !== true || oResults.msg !== "Success") {
            this.loadDeaultWorkFlows();
            oResults.data = this.workFlowSettings
        }
        if (blnDeleteUserSettings === true) {
            tObj.deleteUserSettings();
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readUserSettingsAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#" + tObj.ContainerDivID ), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        oResults.source = "readUserSettingsAjaxErrorCallback";
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.widget = tObj;
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Settings Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.deleteSuccessCallback = function (data) {
        return; //not supported
       

    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        return; //not supported
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        return; //not supported
           }

    // this control only saves the option settings stored in this.workFlowSettings
    // the settings are page/controller specific so the API must be the name of 
    // the controller with the same page control that the widget is configured for
    this.save = function () {
        //debugger;
        var tObj = this;
        this.saveSettings(false);
        if( this.workFlowSections != null){
            $.each(this.workFlowSections, function (index, item) {
                //currently only some objects support saving
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {                    
                    var sControlVariableName = "wdgt" + item.fieldName + "Section"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined'  && ngl.isObject(tPage[sControlVariableName]) ){
                        tPage[sControlVariableName].save();
                    }
                } 
            });
        }

    }

    this.saveSettings = function (blnAsync) {
        //debugger;
        var tObj = this;
        if (typeof (blnAsync) === 'undefined' || blnAsync === null) { blnAsync = true;}
        if (this.SaveUserSettings != true) { return;}
        if (typeof (this.workFlowSettings) !== 'undefined' && ngl.isObject(this.workFlowSettings) === true && ngl.isArray(this.workFlowSettings) === true) {
            //Create a new pageSettingModel
            var pSetting = new PageSettingModel();
            if (isEmpty(this.sNGLCtrlName)){
                pSetting.name = this.IDKey;
            }else {
                pSetting.name = this.sNGLCtrlName;
            }
            pSetting.value = JSON.stringify(this.workFlowSettings);
            if (blnAsync != true) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                return oCRUDCtrl.update(tObj.API + "/PostPageSetting", pSetting, tObj, "saveSettingsSuccessCallback", "saveSettingsAjaxErrorCallback", false);
            } else {
                //kendo.ui.progress($("#" + this.ContainerDivID), true);
                if (ngl.isNullOrWhitespace(tObj.API) === false) {
                    setTimeout(function (tObj) {
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        if (oCRUDCtrl.update(tObj.API + "/PostPageSetting", pSetting, tObj, "saveSettingsSuccessCallback", "saveSettingsAjaxErrorCallback", true) == false) {
                            // kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                        }
                    }, 2000, tObj);
                }
            }
            
        };

    }

    this.deleteUserSettings = function () {
        var tObj = this;
        if (this.SaveUserSettings != true) { return; }
       
            //Create a new pageSettingModel
            var pSetting = new PageSettingModel();
            if (isEmpty(this.sNGLCtrlName)) {
                pSetting.name = this.IDKey;
            } else {
                pSetting.name = this.sNGLCtrlName;
            }
            pSetting.value = '';

            //kendo.ui.progress($("#" + this.ContainerDivID), true);
            if (ngl.isNullOrWhitespace(tObj.API) === false) {
                setTimeout(function (tObj) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    if (oCRUDCtrl.update(tObj.API + "/PostPageSetting", pSetting, tObj, "saveSettingsSuccessCallback", "saveSettingsAjaxErrorCallback", true) == false) {
                        // kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                    }
                }, 2000, tObj);
            }

    }

    this.loadUserSettings = function() {
        if (typeof (this.workFlowSettings) !== 'undefined' && ngl.isObject(this.workFlowSettings) === true && ngl.isArray(this.workFlowSettings) === true) {
            var tObj = this;
            $.each(this.workFlowSettings, function (index, nitem) {
                var oField = tObj.GetFieldItemByFieldID(nitem.fieldID);
                if (oField){
                    oField.fieldDefaultValue = nitem.fieldDefaultValue;
                    oField.fieldVisible = nitem.fieldVisible;
                    oField.fieldReadOnly = nitem.fieldReadOnly;
                    if (typeof (nitem.fieldLockVisibility) !== 'undefined') {
                        oField.fieldLockVisibility = nitem.fieldLockVisibility;
                    } else {
                        oField.fieldLockVisibility = "false";
                    }
                    
                }
            });
        }
    }

    this.clearWdgtHTML = function () {
        //$("#div" + this.IDKey + "Options")
        //var divContainer = $("#" + this.ContainerDivID);
        //if (!divContainer) { return; }
        //divContainer.html('');
     
        //if (!$("#div" + this.IDKey + "Options")) { return; }

        if ($("#div" + this.IDKey + "Selctions")) {
            $("#div" + this.IDKey + "Selctions").empty();
            $("#div" + this.IDKey + "Selctions").remove();
        }
        if ($("#div" + this.IDKey + "Options")) {
            $("#div" + this.IDKey + "Options").empty();
            $("#div" + this.IDKey + "Options").remove();
        }
        //<div id="divid541201902071316371629671Selctions"
        //$("#div" + this.IDKey + "Options").empty();
        //$("#div" + this.IDKey + "Options").html('');
    }

    this.show = function () {
        //debugger;
        var tObj = this;        
        //var divOptions = $("#div" + this.IDKey + "Options");
        if ($("#div" + this.IDKey + "Options").length == 0) {
            try {
                this.loadUserSettings(); //parent must call read first which then calls show
                this.loadHTML();
                this.loadKendo();
                if (typeof (this.dataFields) === 'undefined' || ngl.isObject(this.dataFields) === false || ngl.isArray(this.dataFields) === false) { return; }

                //this.edit(this.data);
            } catch (err) {
                ngl.showErrMsg("Cannot Load Data", err.message, null);
            }
        }

       // kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
    }

    //this read method call readUserSettingsCallBack
    //the readUserSettingsCallBack calls show and then calls 
    //the read mehtod of all child controls
    this.read = function (intControl) {
        //debugger;
        var blnRet = true;
        var tObj = this;
        var sFilter = this.sNGLCtrlName;
        if (isEmpty(sFilter)){
            sFilter = this.IDKey;
        }
        //kendo.ui.progress($("#" + this.ContainerDivID), true);
        if (ngl.isNullOrWhitespace(tObj.API) === false) {
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.filteredRead(tObj.API + "/GetPageSettings", sFilter, tObj, "readUserSettingsSuccessCallback", "readUserSettingsAjaxErrorCallback") == false) {
                    blnRet = false;
                    //kendo.ui.progress($("#" + tObj.ContainerDivID ), false);
                }
                setTimeout(function (tObj) {
                    if (typeof (tObj.workFlowSections) !== 'undefined' && ngl.isObject(tObj.workFlowSections) === true && ngl.isArray(tObj.workFlowSections) === true) {
                        $.each(tObj.workFlowSections, function (index, item) {
                            if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {
                                //build the control name
                                var sControlVariableName = "wdgt" + item.fieldName + "Section";
                                //we use the page variable to access the object
                                //but check if this is available
                                if (typeof (tPage[sControlVariableName]) !== 'undefined' && ngl.isObject(tPage[sControlVariableName])) {
                                    tPage[sControlVariableName].read(intControl);
                                }
                            }
                        });
                    };
                }, 100, tObj);
            }, 1, tObj);
        }

        return blnRet;
    }

    this.executeActions = function (sActionKey) {
        //debugger;
        var blnRet = true;
        var tObj = this;               
        //execute the actions for each workflow section
        if( this.workFlowSections != null){
            $.each(this.workFlowSections, function (index, item) {
                //currently only some objects support saving
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowSectionCtrl) {                    
                    var sControlVariableName = "wdgt" + item.fieldName + "Section"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined'  && ngl.isObject(tPage[sControlVariableName]) ){
                        tPage[sControlVariableName].executeActions(sActionKey);
                    }
                } 
            });
        }
        //check each work flow setting and execute any actions
        if( this.workFlowSettings!= null){
            $.each(this.workFlowSections, function (index, item) {
               
                if (item.fieldDefaultValue === 'true') {
                    //we only process checked items or groups
                    //get this item
                    var dataItem = tObj.GetFieldItemByFieldID(item.fieldID);
                    if (typeof (dataItem) !== 'undefined'  && ngl.isObject(dataItem) ){
                        if (dataItem.fieldWidgetActionKey === sActionKey  && !isEmpty(dataItem.fieldWidgetAction) ){
                            var blnRaiseAction = true;
                            if (!isEmpty(dataItem.fieldAPIReference)){
                                var oCRUDCtrl = new nglRESTCRUDCtrl();
                                var oResults = oCRUDCtrl.executeDataItemAPI(dataItem);
                                blnRaiseAction = oResults.success;
                                if (blnRaiseAction === false){
                                    ngl.showErrMsg(oResults.err.name, oResults.err.message, null);
                                }
                            } 
                            if (blnRaiseAction === true) {
                                if (typeof (tPage["execActionClick"]) !== 'undefined'  && ngl.isFunction(tPage["execActionClick"]) ){
                                    tPage["execActionClick"](dataItem.fieldWidgetAction,tObj.sNGLCtrlName);
                                }
                            }                             
                        }
                    }
                }
                
            });
        }
    }

    this.delete = function () {
        return; //not supported
    }

    this.addNew = function () {
        return; //not supported
    }

    this.edit = function (data) {
        return; //not supported       
    }

    this.close = function (e) {
        return; //not supported       
    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
                                    IDKey,
                                    sParentID,
                                    fieldData,
                                    Theme,
                                    CallBackFunction,
                                    API,
                                    DataType,
                                    DataSourceName,
                                    EditErrorMsg,
                                    EditErrorTitle,
                                    sNGLCtrlName) {
        //debugger;
        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.Theme = Theme;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;
        this.EditErrorMsg = EditErrorMsg;
        this.EditErrorTitle = EditErrorTitle;
        this.sNGLCtrlName = sNGLCtrlName;


    }
}

// NGLWorkFlowSectionCtrls (62) must be associated as a child of a 
// NGLWorkFlowGroup (59) or a NGLWorkFlow...Switch (60 or 61)
// NGLWorkFlowGroup or NGLWorkFlow...Switchs must be associated
// with an NGLWorkFlowOptionCtrl (58)
// WorkFlowSections capture the data associated with a Work Flow Option 
// If the parent of the NGLWorkFlowSectionCtrl is selected the container
// (page or popupwindow) will pass the saved settings to the Page or Popupwindow 
// save API associated with the selected option or option group
// each 
function NGLWorkFlowSectionCtrl() {

    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    //Widget specific properties
    this.DataSourceName = "Record";
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    screenLEName: null;
    IDKey: "id1234";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.PKName = "";
    this.sNGLCtrlName = "";

    //Widget specific functions
    this.ListChanged = function (e, tList) {
        var source = this;
        //NGLEDITOnPageListChanged
        if (typeof (NGLWorkFlowSectionListChanged) !== 'undefined' && ngl.isFunction(NGLWorkFlowSectionListChanged)) {
            NGLWorkFlowSectionListChanged(e, tList, source);
        }
    };

    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }
        
    this.SetFieldDefault = function (sName,sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }
    
    this.GetFieldItem = function (sID){
        var sRet = null;
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItemByFieldID = function (sFieldID){
        var sRet = null;
        if (!sFieldID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldID == sFieldID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.loadHTML = function () {
        var tObj = this;
        //var sHtml = kendo.format("<div id='div{0}Border' class='ngl-blueBorderFullPageWide' style='position: relative; min-width:250px;' ><div id='div{0}Edit' style='padding: 0px 10px 10px 10px;'><div style='margin:5px;'><span id='sp{0}Lbl' style='font-size:large; font-weight:bold'>{1}</span>&nbsp;&nbsp;<a onclick='{2}.save();' class='k-button' href='#'><span class='k-icon k-i-save'></span>Save</a></div>", this.IDKey,this.DataSourceName,this.sNGLCtrlName);
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            tObj.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            var divContainer = $("#" + this.ContainerDivID);
            if (!divContainer) { return false;}
            $("#" + this.ContainerDivID).html("");
            var divThisID = this.IDKey;
            divContainer.append(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div><div id='{1}' style='position:relative;'>",this.IDKey, divThisID));
            var divEdit = $("#" + divThisID);        
            var divActive = divEdit
            var sHT1 = $("#" + this.ContainerDivID).html();
            var $HiddenFields = $("<div style='display:none;'></div>");
            var $InsertOnlyHTML = $("#div" + this.IDKey + "InsertOnly");  //$(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));
            var $floattable = $("<table class='tblResponsive'></table>");
            var $floattableheaderrow = $("<tr></tr>");
            var $floattableitemrow = $("<tr></tr>");
            var floattablecount = 0;
            var sRequiredAsterik = '';
            var sReadOnly = '';
            var bDivOpen = false;
            var bFloatLeftTableOpen = false;
            var bFloatHeadersOpen = false;
            var sFloatTableHeader = "";
            var sFloatTableRow = "";
            var iTableColumns = 0;
            var iTableHeaders = 0;
            var sFormated = "";
            var bInsertOnlyOpen = false;

            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }

                sFormated = "";
                if (item.fieldReadOnly === true) {
                    sReadOnly = '<span class="k-icon k-i-lock"></span>&nbsp;';
                    sRequiredAsterik = '';
                } else {
                    sReadOnly = '';
                    //if (item.fieldAllowNull === false || item.fieldRequired === true) {
                    //Modified by RHR for v-8.2 on 01/02/2019 removed logic to test for allow null on required
                    if (item.fieldRequired === true) {
                        sRequiredAsterik = '<span class="redRequiredAsterik"> *</span>';
                    }
                    else {
                        sRequiredAsterik = '';
                    }
                }

                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  hide hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else {
                    if (item.fieldGroupSubType === nglGroupSubTypeEnum.Span) {
                        sFormated = kendo.format("<span id='sp{3}' style='{0}' class='{1}'>{2}</span>", item.fieldStyle, item.fieldCssClass, item.fieldCaption, item.fieldTagID);
                        domLastID = "sp" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header1) {
                        sFormated = " <h1 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h1>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header2) {
                        sFormated = " <h2 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h2>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header3) {
                        sFormated = " <h3 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h3>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header4) {
                        sFormated = " <h4 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h4>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Paragraph) {
                        sFormated = " <p style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</p>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Link) {
                        sFormated = " <a href='" + item.fieldAPIReference + "'>" + tem.fieldCaption + "</a>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLSpace) {
                        sFormated = "&nbsp;";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLLineBreak) {
                        sFormated = " <br style='clear:both; display: block; float: none;'/>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Div) {
                        //once a div is entered it becomes active and everything below it falls inside this div until   
                        //the next float block, float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because divs are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divActive.append("<div id='div" + item.fieldTagID + "Break' style='padding: 0px 10px 10px 10px; clear:both; display: block; float: none;'></div>");
                        divActive = $("#div" + item.fieldTagID + "Break");
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatLeftTable) {
                        //Note: Once FloatLeftTable is active all fields are added to the table until a 
                        //div a FloatBlockLeft or another FloatLeftTable is activated
                        // fields are added to rows according to the number of tableheader fields included
                        // Labels are used to include text in a row instead of a data object                       
                        if (bFloatLeftTableOpen == true) {
                            //append the open float table to the active div and create a new float table
                            //float tables are always nested inside a <div style="float: left;">
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                        }
                        $floattable = $("<table class='tblResponsive'></table>");
                        $floattableheaderrow = $("<tr></tr>");
                        $floattableitemrow = $("<tr></tr>");
                        floattablecount = floattablecount + 1;
                        iTableColumns = 0;
                        iTableHeaders = 0;
                        bFloatLeftTableOpen = true;
                        bFloatHeadersOpen = false;

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatBlockLeft) {
                        //float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because FloatBlocks are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divEdit.append("<div id='div" + item.fieldTagID + "FloatBlock' style='float:left;'></div>");
                        divActive = $("#div" + item.fieldTagID + "FloatBlock");
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TableHeader) {
                        if (bFloatLeftTableOpen == true) {
                            bFloatHeadersOpen = true;
                            iTableHeaders = iTableHeaders + 1;
                            $floattableheaderrow.append("<th style='border:none;'>" + item.fieldCaption + "</th>");
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Label) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append("<td class='tblResponsive-top'>" + item.fieldCaption + "</td>");
                        } else {
                            //add a Responsive table with just a caption (no data) -- this should not be used
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'></td></tr></table></div>", item.fieldCaption);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox'  /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            //add a Responsive table
                            sFormated = kendo.format(" <div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox' class='k-checkbox' /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    } else if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false && item.fieldMaxlength > 150) {

                        var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";

                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    } else {

                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'>{0}{2}<input id='{3}' {4} /></td>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input id='{3}' {4} /></td></tr></table></div>",
                                 sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    }
                    if (isEmpty(sFormated) === false) {
                        if (item.fieldInsertOnly === true) {
                            $InsertOnlyHTML.append(sFormated);
                        } else {
                            divActive.append(sFormated);
                        }
                    }
                }


            });
            if (bFloatLeftTableOpen == true) {
                //append the open float table to the active div and create a new float table
                //float tables are always nested inside a <div style="float: left;">
                divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                $floattable.append($floattableitemrow);
                $("#divFloatTable-" + floattablecount.toString()).append($floattable);
            }
            //divEdit.append($InsertOnlyHTML) ;   
            divEdit.append($HiddenFields);
            var sHT = $("#" + this.ContainerDivID).html();
        };


    };
    
    this.loadKendo = function () {
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var tObj = this;
            this.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing no kindo widgets for these hidden where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  no kindo widgets for fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMobileSwitch");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoMobileSwitch();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoNumericTextBox");
                    if (!nglkendoitem) {
                        // widget instance does not exist
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoNumericTextBox();
                        };
                    }

                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDatePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDatePicker({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoDatePicker();
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDateTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: item.fieldFormat, interval: 15 });
                        } else {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: "HH:mm ", interval: 15 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoTimePicker();
                        };
                    }

                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoEditor");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoEditor({
                            resizable: {
                                content: false,
                                toolbar: false

                            },
                            // Empty tools so do not display toolbar
                            tools: []
                        });
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoColorPicker");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoColorPicker();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDropDownList");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoDropDownList({
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        $.ajax({
                                            async: true,
                                            type: "GET",
                                            url: "api/" + item.fieldAPIReference + "/" + item.fieldAPIFilterID,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {
                                                options.success(data);
                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get " + item.fieldName + "  Failure");
                                                ngl.showErrMsg("Read " + item.fieldName + " Error", sMsg, null);
                                            }
                                        });
                                    }
                                },
                                schema: {
                                    data: "Data",
                                    total: "Count",
                                    model: {
                                        id: "Control",
                                        fields: {
                                            Name: { editable: false },
                                            Description: { editable: false }
                                        }
                                    },
                                    errors: "Errors"
                                },
                                error: function (e) {
                                    ngl.showErrMsg("Read " + item.fieldName + " Error", e.errors, null);
                                    this.cancelChanges();
                                },
                                serverPaging: false,
                                serverSorting: false,
                                serverFiltering: false
                            },
                            change: function (e) { var tList = this; if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) { tObj.ListChanged(e, tList); } },
                            dataBound: function (e) {
                                var listContainer = e.sender.list.closest(".k-list-container");
                                var iNewWidth = listContainer.width() + 25;
                                listContainer.width(iNewWidth);
                                var tList = this;
                                if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) {
                                    tObj.ListChanged(e, tList);
                                }
                            },
                        });

                        var localDropDown = $("#" + item.fieldTagID).data("kendoDropDownList");
                        localDropDown.list.width("auto");
                    } else {
                        nglkendoitem.dataSource.read();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMaskedTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoMaskedTextBox({ mask: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoMaskedTextBox();
                        };
                    }

                };
            });
        };
    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        oResults.msg = 'Failed'; //set default to Failed   
        var tObj = this;
        oResults.widget = tObj;
        //kendo.ui.progress($(document.body), false);
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            //debugger;
            if (typeof (errWarnData) !== 'undefined') { errWarnData = data; }            
            //if (typeof (wdgtNGLErrWarnMsgLogCtrlDialog) !== 'undefined' && wdgtNGLErrWarnMsgLogCtrlDialog != null) {
            //    wdgtNGLErrWarnMsgLogCtrlDialog.show(data);
            //}
        }
        this.rData = null;
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        blnSuccess = true;
                        oResults.datatype = "bool";
                        oResults.data = data.Data[0];
                        oResults.msg = "Success"
                        //ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);
                    } else {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Unable to save your " + this.DataSourceName + " changes";
                        oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save " + this.DataSourceName + " Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err;
            ngl.showErrMsg(err.name, err.message, null);

        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.msg = 'Failed'; //set default to Failed 
        oResults.widget = tObj;
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.readSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read"
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = this.DataType;
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        //this.show();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                //ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.deleteSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "deleteSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.CRUD = "delete"
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                } else {
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        oResults.datatype = "bool";
                        oResults.data = data.Data;
                        oResults.msg = "Success"
                        //ngl.showSuccessMsg("Success your " + this.DataSourceName + " record has been deleted", tObj);
                        this.addNew();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Unable to delete your " + this.DataSourceName + " record";
                        oResults.error.message = "The delete procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                    try {
                        if (typeof (tObj.ParentIDKey) !== 'undefined' && tObj.ParentIDKey !== null) {
                            var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");
                            grid.dataSource.page(1);
                            grid.dataSource.read();
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.message, null);

                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and check the results.";
                //ngl.showSuccessMsg(oResults.msg, null);
            }
        } catch (err) {
            oResults.error = err
            ngl.showErrMsg(err.name, err.message, null);
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //try {
        //    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //} catch (err) {
        //    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        //}
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed
        oResults.CRUD = "delete"
        oResults.error = new Error();
        oResults.error.name = "Delete " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        var oRet = new validationResults();
        var tObj = this;
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save " + this.DataSourceName + " Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save " + this.DataSourceName + " Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;
            if (typeof (editing) === 'undefined') { this.blnEditing = false; } else { this.blnEditing = editing == "true"; }

            $.each(this.dataFields, function (index, item) {
                if (item.fieldName in data) {
                    //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                    if (
                        (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                        ||
                        (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                    ) {
                        //valid 
                    } else {
                        if (item.fieldRequired === true) {
                            var field = data[item.fieldName];
                            if (isEmpty(field) === true) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                        }
                    }
                }
                //if (tObj.blnEditing === true) { //update changes
                //    if (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)) {
                //        //valid
                //    } else  if ((item.fieldAllowNull === false || item.fieldRequired === true)  && (item.fieldName in data)  ) {
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //} else {
                //    if (item.fieldVisible === false  && item.fieldInsertOnly === false && item.fieldRequired === false ) {
                //        //valid
                //    } else if ((item.fieldAllowNull === false || item.fieldRequired === true )  && (item.fieldName in data)  )  { 
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //}

            });
        }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;

        var divThisSection = $("#div" + tObj.IDKey + "wrapper");
        if (!divThisSection) { return; }
        if (divThisSection.is(':hidden')) { return; }

        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            this.data = new window[this.DataType]; //clear any old data
            //read the fields
            $.each(tObj.dataFields, function (index, item) {
                var field = tObj.data[item.fieldName];
                var elmt = $("#" + item.fieldTagID);
                if (typeof (elmt) !== 'undefined' && elmt != null) {
                    try {

                        if (
                            (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                            ||
                            (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                        ) {
                            //hidden fields                                
                            var hVal = elmt.val();
                            if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                field = ngl.nbrTryParse(hVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                            } else {
                                field = hVal;
                            };
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                            field = elmt.prop("checked");
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                            field = elmt.data("kendoMobileSwitch").check();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                            var numVal = elmt.data("kendoNumericTextBox").value();
                            field = ngl.nbrTryParse(numVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                            field = elmt.data("kendoDatePicker").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                            field = elmt.data("kendoDateTimePicker").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                            field = ngl.convertTimePickerToDateString(elmt.data("kendoTimePicker").value(), null, null);
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                            field = elmt.data("kendoEditor").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                            field = elmt.data("kendoColorPicker").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                            field = elmt.data('kendoDropDownList').value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                            field = elmt.data("kendoMaskedTextBox").value();
                        };
                    } catch (err) {
                        //just read the jquery value
                        var sVal = elmt.val();
                        if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                            field = ngl.nbrTryParse(sVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                        } else {
                            field = sVal;
                        };
                    }
                } else {
                    field = item.fieldDefaultValue;
                }
                tObj.data[item.fieldName] = field;
            });
            var oValidationResults = this.validateRequiredFields(this.data, this.blnEditing);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showErrMsg("Cannot validate " + this.DataSourceName + " Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            } else {
                if (oValidationResults.Success === false) {
                    var wdth = ($(window).width() / 10) * 6;
                    var hgt = ($(window).height() / 10) * 6;
                    ngl.Alert("Required Fields", oValidationResults.Message, wdth, hgt);
                    return;
                }
            }

            //save the changes         

            //kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
            if (ngl.isNullOrWhitespace(tObj.API) === false) {
                // time out removed by RHR because nested saves may be dependent upon these results
                //setTimeout(function (tObj) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    if (oCRUDCtrl.update(tObj.API + "/POST", tObj.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback") == false) {
                        // kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                    }
                //}, 2000, tObj);
            }
        };

    }

    this.show = function () {
        var tObj = this;
        try {
            this.loadHTML();
            this.loadKendo();
            if (typeof (this.dataFields) === 'undefined' || ngl.isObject(this.dataFields) === false || ngl.isArray(this.dataFields) === false) { return; }

            this.edit(this.data);
        } catch (err) {

            ngl.showErrMsg("Cannot Load Data", err.message, null);

        }
        //kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
    }

    this.read = function (intControl) {

        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            //kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
            if (ngl.isNullOrWhitespace(tObj.API) === false) {
                setTimeout(function (tObj) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    if (oCRUDCtrl.read(tObj.API + "/GetByParent" , intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                        blnRet = false;
                        //kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                    }
                }, 1, tObj);
            } else {
                this.show();
            }
        }
        return blnRet;
    }

    this.executeActions = function (sActionKey){
        var blnRet = true;
        var tObj = this;               
        //execute the actions for each item section
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            
            $.each(this.dataFields, function (index, item) {              
             
                if (item.fieldWidgetActionKey === sActionKey  && !isEmpty(item.fieldWidgetAction) ){
                    var blnRaiseAction = true;
                    if (!isEmpty(item.fieldAPIReference)){
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        var oResults = oCRUDCtrl.executeDataItemAPI(item);
                        blnRaiseAction = oResults.success;
                        if (blnRaiseAction === false){
                            ngl.showErrMsg(oResults.err.name, oResults.err.message, null);
                        }
                    } 
                    if (blnRaiseAction === true) {
                        if (typeof (tPage["execActionClick"]) !== 'undefined'  && ngl.isFunction(tPage["execActionClick"]) ){
                            tPage["execActionClick"](item.fieldWidgetAction,tObj.sNGLCtrlName);
                        }
                    }  
                }
                
            });
        }
    }

    this.delete = function () {
        return true; //not supported
        //var blnRet = true;
        //var tObj = this;
        //var intControl
        //if (typeof (this.data) === 'undefined' || ngl.isObject(this.data) === false) {
        //    ngl.showWarningMsg("Cannot Delete", "Invalid data. If this is a new record no changes have been saved so there is nothing to delete.", null);
        //    return false;
        //}
        //if (typeof (this.PKName) === 'undefined' || this.PKName == null || ((this.PKName in this.data) == false)) {
        //    ngl.showWarningMsg("Cannot Delete", "Invalid value for key field " + this.PKName + ". Please check the grid content management settings or contact technical support..", null);
        //    return false;
        //}
        //intControl = this.data[this.PKName];
        //if (typeof (intControl) != 'undefined' && intControl != null) {
        //    this.rData = null;
        //    this.data = null;

        //    //kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
        //    setTimeout(function (tObj) {
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        if (oCRUDCtrl.delete(tObj.API + "/DELETE", intControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback") == false) {
        //            blnRet = false;
        //            //kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        //        }
        //    }, 200, tObj);
        //} else {
        //    ngl.showWarningMsg("Cannot Delete", "Invalid value in key field " + this.PKName + ". If this is a new record no changes have been saved so there is nothing to delete.", null);
        //    return false;
        //}
        //return blnRet;
    }

    this.addNew = function () {
        this.data = null;
        $('#div' + this.IDKey + 'InsertOnly').show();
        this.edit(this.data);
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;
        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            $('#div' + this.IDKey + 'InsertOnly').hide();
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldName in data) {
                        var blnReadOnly = false;
                        if (item.fieldReadOnly === true) { blnReadOnly = true; }
                        var dataItem = data[item.fieldName];
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))) {
                                //no kindo widgets for hidden fields  
                                $("#" + item.fieldTagID).val(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).prop('checked', dataItem);
                                $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").check(dataItem);
                                //$("#" + item.fieldTagID).data("kendoMobileSwitch").value({ checked: dataItem });
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").enable(!blnReadOnly);
                                //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoMobileSwitch").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                $("#" + item.fieldTagID).data("kendoDatePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                $("#" + item.fieldTagID).data("kendoTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                                $("#" + item.fieldTagID).data("kendoEditor").value(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                                $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                $("#" + item.fieldTagID).data("kendoDropDownList").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                            };
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.EditErrorTitle, this.EditErrorMsg, null); this.EditError = true; return false;
            };
        } else {
            $('#div' + this.IDKey + 'InsertOnly').show();
            //we are adding a new record
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    var blnReadOnly = false;
                    if (item.fieldInsertOnly === false && item.fieldReadOnly === true && item.fieldRequired === false) { blnReadOnly = true; }
                    var blnHasDefaultVal = false;
                    if (typeof (item.fieldDefaultValue) !== 'undefined' && item.fieldDefaultValue !== null && isEmpty(item.fieldDefaultValue) === false) { blnHasDefaultVal = true; }
                    if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)) {
                        //no kindo widgets for hidden fields  
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).val(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).val();
                        };
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue == "true") {
                                $("#" + item.fieldTagID).prop('checked', true);
                            } else {
                                $("#" + item.fieldTagID).prop('checked', false);
                            }

                        } else {
                            $("#" + item.fieldTagID).prop('checked', false);
                        }
                        $("#" + item.fieldTagID).prop("disabled", blnReadOnly);

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoMobileSwitch) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue == "true") {
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").check(true);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMobileSwitch").check(false);
                            }

                        } else {
                            $("#" + item.fieldTagID).data("kendoMobileSwitch").check(false);
                            //$("#" + item.fieldTagID).data("kendoMobileSwitch").value({ checked: false });
                        }
                        $("#" + item.fieldTagID).data("kendoMobileSwitch").enable(!blnReadOnly);
                        //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly); 
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoEditor").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoEditor").value();
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDropDownList").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoDropDownList").select(0);
                        }
                        $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                    };
                });
            } else {
                ngl.showValidationMsg(this.AddErrorTitle, this.AddErrorMsg, null); this.EditError = true; return false;
            };
        };
        return true;
    }

    this.close = function (e) {


    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
                                    IDKey,
                                    sParentID,
                                    fieldData,
                                    Theme,
                                    CallBackFunction,
                                    API,
                                    DataType,
                                    DataSourceName,
                                    EditErrorMsg,
                                    EditErrorTitle,
                                    sNGLCtrlName) {

        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.Theme = Theme;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;
        this.EditErrorMsg = EditErrorMsg;
        this.EditErrorTitle = EditErrorTitle;
        this.sNGLCtrlName = sNGLCtrlName;


    }

}

// requires the expandFastTab and collapseFastTab functions in NGL's core.js
// identified by the "Form Fast Tab" group sub type 22
// the first child object is the header div and the second child object is the details div
// only two children are allowed, any of the following children are allowed:
// NGLWorkFlowSectionCtrl,NGLWorkFlowSectionCtrl,Standard Border,Float Block Left,
// Two Column Data,Paragraph,Images, HTML Div, NGLEditOnPageCtrl,FloatLeftTable
function NGLFastTabCtrl() {
    data: null;
    callback: null;
    IDKey: "id1234";
    this.Theme = "blueopal";
    HeaderID: "1234";
    DetailID: null;
    ExpandID: "";
    CollapseID: "";
    ContainerDivID: '';
    this.Expanded = true;
    this.Caption = "Details";
    this.PageControl = 0;
    this.UserSettingKey = "";

    this.loadHTML = function () {
        this.appendToContainer(this.IDKey, this.ContainerDivID, this.Expanded, this.HeaderID, this.DetailID, this.Caption);
        //var divContainer = $("#" + this.ContainerDivID);
        //if (!divContainer) {return;}
        //var cssDisplayStyle = "display: normal;";
        //if (this.Expanded != true){cssDisplayStyle = "display: none;";}
        //var sExtendID = "'Expand" + this.IDKey + "Span'";
        //var sCollapseID = "'Collapse" + this.IDKey + "Span'";
        //var sHeaderID = "null";
        //if (this.HeaderID != null){ sHeaderID = "'" + this.HeaderID + "'";}
        //var sDetailID = "null";
        //if (this.DetailID != null){ sDetailID = "'" + this.DetailID + "'";}
        
        //divContainer.append(kendo.format("<span id={0} style='{1}'><a onclick=\"expandFastTab({0}, {2}, {3}, {4});\"><span class='k-icon k-i-arrow-chevron-down ui-span-container'></span></a></span>",sExtendID,cssDisplayStyle,sCollapseID,sHeaderID,sDetailID));
        //if (this.Expanded === true){cssDisplayStyle = "display: none;";} else {cssDisplayStyle = "display: normal;"; }
        //divContainer.append(kendo.format("<span id={2} style='{1}'><a onclick=\"collapseFastTab({0}, {2}, {3}, {4});\"><span class='k-icon k-i-arrow-chevron-up ui-span-container'></span></a></span>",sExtendID,cssDisplayStyle,sCollapseID,sHeaderID,sDetailID));
        //divContainer.append(kendo.format("<span style='font-size:small; font-weight:bold'>{0}</span>",this.Caption));           
    }

    this.appendToContainer = function (sId, containerDivID, blnexpanded, headerDivID, detailDivID, sCaption) {
        if (!containerDivID) { return;}
        var divContainer = $("#" + containerDivID);
        if (!divContainer) { return; } 

        this.ExpandID = "Expand" + sId + "Span"
        //var spnExpand = $("#" + this.ExpandID);
        //if (!spnExpand) {
        if ($("#Expand" + sId + "Span").length == 0) {
                this.IDKey = sId;
                this.ContainerDivID = containerDivID;
                this.Expanded = blnexpanded;
                this.HeaderID = headerDivID;
                this.DetailID = detailDivID;
                this.Caption = sCaption;
                var cssDisplayStyle = "display: none;";
                if (blnexpanded != true) { cssDisplayStyle = "display: normal;"; }
                
                var sExtendID = "'" + this.ExpandID + "'";
                this.CollapseID = "Collapse" + sId + "Span";
                var sCollapseID = "'" + this.CollapseID + "'";
                var sHeaderID = "null";
                if (headerDivID) {
                    sHeaderID = "'" + headerDivID + "'";
                }
      
                var sDetailID = "null";
                if (detailDivID) {
                    sDetailID = "'" + detailDivID + "'";
                }
                divContainer.append(kendo.format("<span id={0} style='{1}'><a onclick=\"expandFastTab({0}, {2}, {3}, {4});\"><span class='k-icon k-i-arrow-chevron-down ui-span-container'></span></a></span>", sExtendID, cssDisplayStyle, sCollapseID, sHeaderID, sDetailID));
                if (blnexpanded === true) {
                    this.Expanded = true;
                    cssDisplayStyle = "display: normal;";
                } else {
                    this.Expanded = false;
                    cssDisplayStyle = "display: none;";
                }
                divContainer.append(kendo.format("<span id={2} style='{1}'><a onclick=\"collapseFastTab({0}, {2}, {3}, {4});\"><span class='k-icon k-i-arrow-chevron-up ui-span-container'></span></a></span>", sExtendID, cssDisplayStyle, sCollapseID, sHeaderID, sDetailID));
                divContainer.append(kendo.format("<span style='font-size:small; font-weight:bold'>{0}</span>", sCaption));
                //if (this.Expanded === true) {
                //    expandFastTab("Expand" + sId + "Span", "Collapse" + sId + "Span", headerDivID, detailDivID);
                //} else {
                //    collapseFastTab("Expand" + sId + "Span", "Collapse" + sId + "Span", headerDivID, detailDivID);
                        //}
            }
    }

    this.Expand = function(){
        this.Expanded = true;
        expandFastTab(this.ExpandID, this.CollapseID, this.HeaderID, this.DetailID);
    }

    this.Collapse = function () {
        this.Expanded = false;
        collapseFastTab(this.ExpandID, this.CollapseID, this.HeaderID, this.DetailID);
    }

    this.Toggle = function () {
        if (this.Expanded === true) {
            this.Collapse();
        } else {
            this.Expand();
        }
    }

    this.show = function () {
        this.loadHTML();
    }
    //note: CallBackFunction,PageControl,UserSettingKey are not currently used.
    //      they may be passed to the expand and collapse functions in the future
    //      UserSettingKey will be used to save the expanded and collapsed value to the db
    //      for the pagecontrol number so UserSettingKey should be unique
    this.loadDefaults = function (ContainerDivID,
                                    IDKey,
                                    Theme,
                                    CallBackFunction,
                                    HeaderID,
                                    DetailID,
                                    Expanded,
                                    Caption,
                                    sNGLCtrlName,
                                    PageControl,
                                    UserSettingKey) {

        IDKey: "id1234";
        this.Theme = "blueopal";
        HeaderID: "1234";
        DetailID: null;
        ContainerDivID: '';
        this.Expanded = true;
        this.Caption = "Details";

        var tObj = this;
        this.callback = CallBackFunction;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.Theme = Theme;
        this.HeaderID = HeaderID;
        this.DetailID = DetailID;
        this.Expanded = Expanded;
        this.Caption = Caption;
        this.sNGLCtrlName = sNGLCtrlName;
        this.PageControl = PageControl;
        this.UserSettingKey = UserSettingKey;


    }
}

////test code represents Spot Rate details data
//var objSpotRateDetailsDataFields = [

//{ fieldID: "1000", fieldTagID: "txtSpotRateCarrierControl", fieldCaption: "Select Provider", fieldName: "SpotRateCarrierControl", fieldDefaultValue: "", fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "", fieldAllowNull: true, fieldVisible: true, fieldRequired: true, fieldMaxlength: 50, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetUserDynamicList", fieldAPIFilterID: "14" }


//, { fieldID: "1001", fieldTagID: "txtSpotRateLineHaul", fieldCaption: "Enter Line Haul", fieldName: "SpotRateLineHaul", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:c2}", fieldAllowNull: false, fieldVisible: true, fieldRequired: true, fieldMaxlength: 20, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "" }


//, { fieldID: "1002", fieldTagID: "txtSpotRateAllocationFormula", fieldCaption: "Allocation Formula", fieldName: "SpotRateAllocationFormula", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: true, fieldFormat: "", fieldAllowNull: true, fieldVisible: true, fieldRequired: false, fieldMaxlength: 50, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "" }

////];

//var objSpotRateOptionsDataFields = [

//{ fieldID: "1004", fieldTagID: "FlatRate-switch", fieldCaption: "Flat Rate", fieldName: "FlatRate", fieldDefaultValue: "", fieldGroupSubType: 61, fieldReadOnly: false, fieldFormat: "", fieldAllowNull: true, fieldVisible: true, fieldRequired: false, fieldMaxlength: 50, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: 0, fieldSequenceNo: 0 }


//, { fieldID: "1005", fieldTagID: "txtSpotRateLineHaul", fieldCaption: "Enter Line Haul", fieldName: "SpotRateLineHaul", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:c2}", fieldAllowNull: false, fieldVisible: true, fieldRequired: true, fieldMaxlength: 20, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: 0, fieldSequenceNo: 1 }


//, { fieldID: "1006", fieldTagID: "txtSpotRateAllocationFormula", fieldCaption: "Allocation Formula", fieldName: "SpotRateAllocationFormula", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: true, fieldFormat: "", fieldAllowNull: true, fieldVisible: true, fieldRequired: false, fieldMaxlength: 50, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: 0, fieldSequenceNo: 2 }

//];
//divSpotRateWrapper
//Expanded = false
//ExpandSpotRateDetailsSpan
//CollapseSpotRateDetailsSpan
//divSpotRateDetails
//SpotRateDetailsID = divSpotRateDetails
//SpotRateFastTabID = ftdivSpotRateDetails



function WhatsNewReportCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: Dispatch();
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    strAdditionalServices: "";
    //Widget specific properties

    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions

    this.readSuccessCallback = function (data) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read BOL Data Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "Dispatch";
                        oResults.msg = "Success"
                        this.data = data.Data;
                        if (typeof (this.data[0].BookControl) !== 'undefined' && this.data[0].BookControl != null) {
                            this.readAdditionalServices(this.data[0].BookControl);
                        }
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else { oResults.msg = "Success but no data was returned. Please refresh your page and try again."; ngl.showSuccessMsg(oResults.msg, tObj); }
        } catch (err) { oResults.error = err; }
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
        if (oResults.msg == "Success") { this.show(); }
    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read BOL Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }


    this.print = function () {
        var kinWin = $("#winBOLRpt").kendoWindow().data("kendoWindow");
        var winWrapper = kinWin.wrapper;
        winWrapper.removeClass("k-window");
        winWrapper.addClass("printable");
        //put pring code here
        window.print();
        winWrapper.removeClass("printable");
        winWrapper.addClass("k-window");
    }

    this.email = function () {
        var docTitle = "BOL-" + this.data[0].SHID + ".pdf"
        var tObj = this;

        ////kendo.drawing.drawDOM("#winBOLRpt", { paperSize: "A4", forcePageBreak: ".pgbrk" }).then(function (group) { kendo.drawing.pdf.saveAs(group, docTitle) });

        var draw = kendo.drawing;
        draw.drawDOM($("#winBOLRpt")).then(function (root) {
            return draw.exportPDF(root, { paperSize: "A4", forcePageBreak: ".pgbrk" }); // Chaining the promise via then
        }).done(function (dataURI) {
            //Extracting the base64-encoded string and the contentType
            var data = {};
            var parts = dataURI.split(";base64,");
            //data.contentType = parts[0].replace("data:", "");
            //data.base64 = parts[1];

            var gr = new GenericResult();
            gr.strField = parts[0].replace("data:", "");
            gr.strField2 = parts[1];
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            oCRUDCtrl.update("BOL/EmailBOL", gr, tObj, "emailBOLSuccessCallback", "emailBOLAjaxErrorCallback");

            ////var blob;
            ////var BASE64_MARKER = ';base64,';
            ////if (dataURI.indexOf(BASE64_MARKER) == -1) {
            ////    var parts = dataURI.split(',');
            ////    var contentType = parts[0].split(':')[1];
            ////    var raw = decodeURIComponent(parts[1]);
            ////    blob = new Blob([raw], { type: contentType });
            ////}
            ////else {
            ////    var parts = dataURI.split(BASE64_MARKER);
            ////    var contentType = parts[0].split(':')[1];
            ////    var raw = window.atob(parts[1]);
            ////    var rawLength = raw.length;
            ////    var uInt8Array = new Uint8Array(rawLength);
            ////    for (var i = 0; i < rawLength; ++i) {
            ////        uInt8Array[i] = raw.charCodeAt(i);
            ////    }
            ////    blob = new Blob([uInt8Array], { type: contentType });
            ////}

        });
    }

    this.emailBOLSuccessCallback = function (data) {
        //var oResults = new nglEventParameters();
        var tObj = this;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Email BOL Data Failure", data.Errors, tObj); }
                else {
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (ngl.stringHasValue(data.Data[0])) { ngl.showSuccessMsg(data.Data[0], tObj); }
                    }
                    else { ngl.showErrMsg("Invalid Request", "No Data available.", tObj); }
                }
            } else { ngl.showSuccessMsg("Success but no data was returned. Please refresh your page and try again.", tObj); }
        } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
    }

    this.emailBOLAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "emailBOLAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Email BOL Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
    }

    this.show = function () {
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }

        var tObj = this;
        this.kendoWindowsObj = $("#winBOLRpt").kendoWindow({
            title: "Bill of Lading Report",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            scrollable: true,
            //actions: ["email","print", "Minimize", "Maximize", "Close"],
            actions: ["print", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#winBOLRpt").data("kendoWindow").wrapper.find(".k-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });
            //$("#winBOLRpt").data("kendoWindow").wrapper.find(".k-i-email").parent().click(function (e) { e.preventDefault(); tObj.email(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("BOL/GetBOL", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }
        return blnRet;
    }

    this.fixIds = function (elem, cntr) {
        $(elem).find("[id]").add(elem).each(function () {
            this.id = this.id + cntr;
        });
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        $("#divBOLRpt").show();
        $("#divErrMsgBOLRpt").hide();
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            if (typeof (this.data) !== 'undefined' && ngl.isArray(this.data) && this.data.length > 0) {

                if (typeof (data[0]) === 'undefined' || !ngl.isObject(data[0])) { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); return; }
                if (data[0].BookControl === 0 && data[0].LoadTenderControl === 0 && !ngl.stringHasValue(data[0].ProviderSCAC)) { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); return; }
                for (i = 1; i < 100; i++) {
                    var sID = "divBOLRpt" + i.toString();
                    var sPbrID = "pgbr" + i.toString();
                    if ($("#" + sID).length) {
                        $("#" + sID).remove();
                        if ($("#" + sPbrID).length) {
                            $("#" + sPbrID).remove();
                        }
                    } else {
                        break;
                    }
                }


                var strIx = "";
                for (i = 0; i < data.length; i++) {
                    if (typeof (data[i]) !== 'undefined' && ngl.isObject(data[i])) {

                        if (i > 0) {
                            var clone = $("#divBOLRpt").clone(true, true);
                            this.fixIds(clone, i);
                            clone.insertAfter("#pgbr" + strIx); //must be first
                            strIx = i.toString(); //after intsertAfter
                            if (i != (data.length - 1)) {
                                $("<h1 id='pgbr" + strIx + "' class='pgbrk'></h1>").insertAfter("#divBOLRpt" + strIx); //must be last
                            }
                        }

                        $("#spBOLRptSHID" + strIx).html(data[i].SHID);
                        $("#spBOLRptBOL" + strIx).html(data[i].BillOfLading);
                        $("#spBOLRptCarrierName" + strIx).html(data[i].CarrierName);
                        $("#spBOLRptCarrierSCAC" + strIx).html(data[i].ProviderSCAC);
                        $("#spBOLRptPickupDate" + strIx).html(ngl.getShortDateString(data[i].PickupDate));
                        $("#spBOLRptOrigName" + strIx).html(data[i].Origin.Name);
                        $("#spBOLRptOrigAddress1" + strIx).html(data[i].Origin.Address1);
                        $("#spBOLRptOrigCity" + strIx).html(data[i].Origin.City);
                        $("#spBOLRptOrigState" + strIx).html(data[i].Origin.State);
                        $("#spBOLRptOrigZip" + strIx).html(data[i].Origin.Zip);
                        $("#spBOLRptOrigContactName" + strIx).html(data[i].Origin.Contact.ContactName);
                        $("#spBOLRptOrigPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Origin.Contact.ContactPhone));
                        $("#spBOLRptOrigEmail" + strIx).html(data[i].Origin.Contact.ContactEmail);
                        $("#spBOLRptDeliveryDate" + strIx).html(ngl.getShortDateString(data[i].DeliveryDate));
                        $("#spBOLRptDestName" + strIx).html(data[i].Destination.Name);
                        $("#spBOLRptDestAddress1" + strIx).html(data[i].Destination.Address1);
                        $("#spBOLRptDestCity" + strIx).html(data[i].Destination.City);
                        $("#spBOLRptDestState" + strIx).html(data[i].Destination.State);
                        $("#spBOLRptDestZip" + strIx).html(data[i].Destination.Zip);
                        $("#spBOLRptDestContactName" + strIx).html(data[i].Destination.Contact.ContactName);
                        $("#spBOLRptDestPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Destination.Contact.ContactPhone));
                        $("#spBOLRptDestEmail" + strIx).html(data[i].Destination.Contact.ContactEmail);
                        $("#spBOLRptShipWgt" + strIx).html(data[i].TotalWgt);
                        $("#spBOLRptShipQty" + strIx).html(data[i].TotalQty);
                        $("#spBOLRptShipPlts" + strIx).html(data[i].TotalPlts);
                        $("#spBOLRptShipCubes" + strIx).html(data[i].TotalCube);
                        var strInstructions = "";
                        if (ngl.isNullOrWhitespace(data[i].PickupNote) === false) { strInstructions = "<b>Pickup:</b>&nbsp;" + data[i].PickupNote + "&nbsp;&nbsp;"; }
                        if (ngl.isNullOrWhitespace(data[i].DeliveryNote) === false) { strInstructions = strInstructions + " <b>Delivery:</b>&nbsp; " + data[i].DeliveryNote; }
                        $("#spBOLRptSpecialInstructions" + strIx).html(strInstructions);
                        $("#spBOLRptQuoteNumber" + strIx).html(data[i].QuoteNumber);
                        $("#spBOLRptConfNbr" + strIx).html(data[i].PickupNumber);
                        $("#spBOLRptPONumber" + strIx).html(data[i].PONumber);
                        $("#spBOLRptOrderNumber" + strIx).html(data[i].OrderNumber);
                        $("#spBOLRptCarrierProNumber" + strIx).html(data[i].CarrierProNumber);
                        $("#spBOLRptItemOrderNumbers" + strIx).html(data[i].ItemOrderNumbers);
                        $("#spBOLRptBillToName" + strIx).html(data[i].Requestor.Name);
                        $("#spBOLRptBillToAddress1" + strIx).html(data[i].Requestor.Address1);
                        $("#spBOLRptBillToCity" + strIx).html(data[i].Requestor.City);
                        $("#spBOLRptBillToState" + strIx).html(data[i].Requestor.State);
                        $("#spBOLRptBillToZip" + strIx).html(data[i].Requestor.Zip);
                        $("#spBOLRptBillToPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Requestor.Contact.ContactPhone));
                        $("#spBOLRptBillingType" + strIx).html("Prepaid");
                        $("#spBOLRptLegal" + strIx).html("<small>" + data[i].BOLLegalText + "</small>");
                        var arrItems = data[i].Items;

                        $("#divBOLRptItemDetails" + strIx).kendoGrid({
                            dataSource: {
                                data: arrItems,
                                schema: {
                                    model: {
                                        id: "ItemNumber",
                                        fields: {
                                            "ItemNumber": { type: "string", visible: true, editable: false, nullable: true },
                                            "ItemWgt": { type: "number", defaultValue: 1 },
                                            "ItemFreightClass": { type: "string", defaultValue: "100", editable: false, nullable: true },
                                            "ItemPackageType": { type: "string", defaultValue: "PLT", editable: false, nullable: true },
                                            "ItemTotalPackages": { type: "number", defaultValue: 1 },
                                            "ItemPieces": { type: "number", defaultValue: 1 },
                                            "ItemDesc": { type: "string", editable: true, nullable: true },
                                            "ItemDimensions": { type: "string" },
                                            "ItemNMFCItemCode": { type: "string", editable: false, nullable: true },
                                            "ItemNMFCSubCode": { type: "string", editable: false, nullable: true },
                                            "ItemCube": { type: "number", defaultValue: 1 }
                                        }
                                    }
                                },
                                aggregate: [
                                    { field: "ItemPieces", aggregate: "sum" },
                                    { field: "ItemTotalPackages", aggregate: "sum" },
                                    { field: "ItemWgt", aggregate: "sum" },
                                    { field: "ItemCube", aggregate: "sum" }
                                ]
                            },
                            scrollable: false,
                            columns: [
                                { field: "ItemDesc", title: "Description", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "Totals:", width: 80 },
                                { field: "ItemPieces", title: "Qty", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 20 },
                                { field: "ItemPackageType", title: "Pkg", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, template: "#: ItemTotalPackages # #: ItemPackageType #", footerTemplate: "#= kendo.toString(data.ItemTotalPackages.sum)#", width: 35 },
                                { field: "ItemWgt", title: "Wgt", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                { field: "ItemCube", title: "Cube", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                { field: "ItemNumber", title: "Ref", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                { field: "ItemDimensions", title: "Dims (lwh)", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 50 },
                                { field: "ItemFreightClass", title: "FAK", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 },
                                { field: "ItemNMFCItemCode", title: "NMFC", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                { field: "ItemNMFCSubCode", title: "Sub", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 }
                                ////{ field: "ItemPieces", title: "Qty", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 20 },
                                ////{ field: "ItemPackageType", title: "Pkg", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, template: "#: ItemTotalPackages # #: ItemPackageType #", footerTemplate: "#= kendo.toString(data.ItemTotalPackages.sum)#", width: 35 },
                                ////{ field: "ItemDesc", title: "Description", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 80 },
                                ////{ field: "ItemNumber", title: "Ref", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                ////{ field: "ItemDimensions", title: "Dims (lwh)", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 50 },
                                ////{ field: "ItemWgt", title: "Wgt", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                ////{ field: "ItemCube", title: "Cube", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                ////{ field: "ItemFreightClass", title: "FAK", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 },
                                ////{ field: "ItemNMFCItemCode", title: "NMFC", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                ////{ field: "ItemNMFCSubCode", title: "Sub", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 }
                            ]
                        });

                        $('#divBOLRptItemDetails' + strIx).data('kendoGrid').dataSource.read();

                        var strAccessorials = "";
                        var arrAccessorials = data[i].Accessorials;
                        if (ngl.isArray(arrAccessorials)) {
                            var sSep = "";
                            for (j = 0; j < arrAccessorials.length; j++) {
                                strAccessorials += (sSep + arrAccessorials[j]);
                                sSep = ", ";
                            }
                        }
                        if (!ngl.stringHasValue(strAccessorials)) { strAccessorials = "None"; }
                        //this.strAdditionalServices = data.Data[0];
                        $("#spBOLRptAdditionalServices" + strIx).html(strAccessorials);
                    }
                }
            } else { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); }
        } else { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); }
    }

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        //var oResults = new nglEventParameters();
        //oResults.source = "close";
        //oResults.widget = this;
        //oResults.msg = 'closing nothing is saved';
        //if (typeof (this.onClose) === "function") { this.onClose(oResults); }
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {
        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            //this is the add new popup window
            this.kendoWindowsObj = pageVariable;
        } else { this.kendoWindowsObj = null; }
    }
}




//reuseable object that waits until x independent asynchronous callbacks finish executing before firing an event
function nglAjaxCBWaitCtrl() {
    onRequestsCompleted: null;
    this.numRequestToComplete = 0;
    this.requestsCompleted = 0;
    //this.callBacks: null;

    /* The user can set how many responses to wait for */
    this.setRequestCount = function (requestCount) {
        numRequestToComplete = requestCount;
    }

    /* Sets the number of responses to wait for to 0 */
    this.resetRequestCount = function () {
        numRequestToComplete = 0;
    }

    /* Sets the number of requests completed to 0 */
    this.resetRequestCompleteCount = function () {
        requestsCompleted = 0;
    }

    /* Call this method from an external asychronous callback method to increment the count of requests completed */
    this.requestComplete = function (isComplete) {
        if (isComplete) requestsCompleted++;
        if (requestsCompleted == numRequestToComplete) fireCallbacks();
    }

    /* Calls the external callback method when all the requests are completed */
    this.fireCallbacks = function () {
        //for (var i = 0; i < callBacks.length; i++) callBacks[i]();
        if (ngl.isFunction(this.onRequestsCompleted)) { this.onRequestsCompleted(); }
    }

    /* The user can set the external callback method to fire when all the requests are completed */
    this.setCallback = function (cbOnRequestsCompleted) {
        //callBacks.push(callBack);
        this.onRequestsCompleted = cbOnRequestsCompleted;
    }

    //this.addCallbackToQueue = function (isComplete, callback) {
    //    if (isComplete) requestsCompleted++;
    //    if (callback) callBacks.push(callback);
    //    if (requestsCompleted == numRequestToComplete) fireCallbacks();
    //}

    /* Sets up the callback cbOnRequestsCompleted and property intRequestsToComplete. All call backs return a reference to this object and a string message as parameters */
    this.loadDefaults = function (pageVariable, cbOnRequestsCompleted, intRequestsToComplete) {
        this.onRequestsCompleted = cbOnRequestsCompleted;
        this.numRequestToComplete = intRequestsToComplete;
        this.requestsCompleted = 0;       
    }

}


//reusable widget that creates a window with a selection grid for choosing items to create a bitwise flag
function SelectionGridWndCtrl() { //Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
    //Generic properties for all Widgets
    onSave: null;
    kendoWindowsObj: null;
    //Widget specific properties
    API: "";
    ContainerDivID: "";
    WindowTitle: "";

    this.kendoWindowsObjUploadEventAdded = 0;

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        kendo.ui.progress($("#" + this.ContainerDivID), false);
        oResults.CRUD = this.CRUD;
        ////this.rData = null;
        try { kendo.ui.progress(tObj.kendoWindowsObj.element, false); } 
        catch (err) { ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null); }
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                }
                else {
                    ////this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        blnSuccess = true;
                        var dataType = typeof (data.Data[0]);
                        oResults.datatype = dataType;
                        oResults.data = data.Data;
                        oResults.msg = "Success"
                        //ngl.showSuccessMsg("Success your changes have been saved", null);                      
                        this.kendoWindowsObj.close(); //Only close the window if the save was successful
                        //ngl.showSuccessMsg(oResults.msg, tObj);
                    } else {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Unable to save your changes";
                        oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }
                }
            }
            if (blnSuccess === false) {
                if (blnErrorShown === false) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Failure";
                    oResults.error.message = "No results are available.";
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                };
            };
        } catch (err) { oResults.error = err; ngl.showErrMsg(err.name, err.message, null); }
        if (ngl.isFunction(this.onSave)) { this.onSave(oResults); }
    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        try { kendo.ui.progress(tObj.kendoWindowsObj.element, false); } 
        catch (err) { ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null); }
        kendo.ui.progress($("#" + this.ContainerDivID), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = this.CRUD;
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        if (ngl.isFunction(this.onSave)) { this.onSave(oResults); }
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;
        var selectedRecords = $("#selectionGrid").data("kendoGrid").selectedKeyNames(); //get selected records
        if (typeof (selectedRecords) !== 'undefined' && ngl.isArray(selectedRecords) && selectedRecords.length > 0) {
            selectGridSave = new SelectableGridSave();
            selectGridSave.BitPositionsOn = selectedRecords;
            selectGridSave.Control = $("#txtSelectionControl").val();
            var saveUrl = 'api/' + this.API + "/Post";
            if (typeof (selectGridSave) !== 'undefined' && ngl.isArray(selectGridSave.BitPositionsOn) && selectGridSave.BitPositionsOn.length > 0) {
                //save the changes         
                kendo.ui.progress($("#" + this.ContainerDivID), true);
                setTimeout(function (tObj) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    if (oCRUDCtrl.update(tObj.API + "/POST", selectGridSave, tObj, "saveSuccessCallback", "saveAjaxErrorCallback") == false) {
                        kendo.ui.progress($("#" + this.ContainerDivID), false);
                    }
                }, 2000, tObj);
            }
        }
    }

    //Widget specific functions
    this.refresh = function () {
        try {
            if (typeof ($("#selectionGrid").data("kendoGrid")) !== 'undefined' && $("#selectionGrid").data("kendoGrid") != null) { $("#selectionGrid").data("kendoGrid").clearSelection(); }
            ngl.readDataSource($('#selectionGrid').data('kendoGrid'));
        } catch (err) { ngl.showErrMsg("Cannot refresh SelectionGridWndCtrl Grid", err.message, null); }
    }

    this.loadHTML = function () {
        //alert($.now() + " " + $.now().toString());
        var tObj = this;
        var sHtml = '<div id="wndSelectionGrid"><a id="focusCancel" href="#"></a><div><div style="padding: 0px 10px 0px 10px;"><div id="selectionGrid"></div><input id="txtSelectionControl" type="hidden"/></div></div></div>';
        $("#" + this.ContainerDivID).html(sHtml);
    };

    this.loadKendo = function () {
        var readUrl = 'api/' + this.API + "/GET/0";
        $("#selectionGrid").kendoGrid({
            autoBind: false,
            dataSource: {
                pageSize: 15,
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: readUrl,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                options.success(data);
                                if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' && data.Errors != null) { ngl.showErrMsg('Access Denied', data.Errors, null); }
                            },
                            error: function (result) { options.error(result); }
                        });
                    },
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "SGItemBitPos",
                        fields: {
                            SGItemBitPos: { type: "number" },
                            SGItemCaption: { type: "string" },
                            SGItemOn: { type: "boolean" }
                        }
                    }
                }
            },
            pageable: true,
            scrollable: false,
            persistSelection: true,
            sortable: true,
            columns: [
                { selectable: true, width: 50 },
                { field: "SGItemCaption", title: "Name", hidden: false },
                { field: "SGItemBitPos", title: "BitPos", hidden: true },
                { field: "SGItemOn", title: "On", hidden: true }
            ],
            dataBound: function (e) {
                var view = this.dataSource.view();
                for (var i = 0; i < view.length; i++) {
                    if (typeof (view[i].SGItemOn) !== 'undefined' && view[i].SGItemOn != null && view[i].SGItemOn === true) {
                        this.tbody.find("tr[data-uid='" + view[i].uid + "']")
                        .addClass("k-state-selected")
                        .find(".k-checkbox")
                        .attr("checked", "checked");
                    }
                }
            }
        });
    };

    this.show = function () {
        var tObj = this;
        var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
        if (!nglkendoWindow) {
            try {
                this.loadHTML();
                this.loadKendo();
                var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
                if (!nglkendoWindow) {
                    this.kendoWindowsObj = $("#" + this.ContainerDivID).kendoWindow({
                        title: this.WindowTitle,
                        modal: true,
                        visible: false,
                        height: '75%',
                        width: '75%',
                        actions: ["save", "Minimize", "Maximize", "Close"],
                        close: function (e) { tObj.close(e); },
                    }).data("kendoWindow");
                }
                try { $("#" + this.ContainerDivID).data("kendoWindow").wrapper.find(".k-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); }); }
                catch (err) { ngl.showInfoNotification("Save not available", "Could not load save method please reload the page to save", null); }
            } catch (err) { ngl.showErrMsg("Cannot show Window", err.message, null); }
        }
        $("#selectionGrid").data("kendoGrid").clearSelection();
        ngl.readDataSource($('#selectionGrid').data('kendoGrid'));
        $("#" + this.ContainerDivID).data("kendoWindow").center().open()
    }

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        //var oResults = new nglEventParameters();
        //oResults.source = "close";
        //oResults.widget = this;
        //oResults.msg = 'closing nothing is saved';
        //if (typeof (this.onClose) === "function") { this.onClose(oResults); }
    }

    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (ContainerDivID, wndTitle, API, saveCallBack) {
        this.onSave = saveCallBack;
        var tObj = this;
        this.API = API;
        this.ContainerDivID = ContainerDivID;
        this.WindowTitle = wndTitle;
    }
}


function NGLErrWarnMsgLogCtrl() {

    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    AsyncMessagesPossible: false;
    AsyncMessageKey: 0;
    AsyncTypeKey: 0;
    //Widget specific properties
    // This is a read only control which does not use API
    // to read the data.  The caller must pass in the data to the show 
    // method which checks for specific data objects all of type NGLMessage
    // log, Message,Warn, and Err
    // each type can have an optional title stored in
    // LogTitle,MsgTitle,WarningTitle,ErrTitle
    // The content managment c# method will load all the default HTML
    // on the page and the object definition.  Additional fields are not available
    // in this version. 
    this.logGridData = [{ Message: "No Data", ErrorDetails: "", ErrorMessage: "", ErrorReason: "" }];
    this.msgGridData = [{ Message: "No Data", ErrorDetails: "", ErrorMessage: "", ErrorReason: "" }];
    this.warnGridData = [{ Message: "No Data", ErrorDetails: "", ErrorMessage: "", ErrorReason: "" }];
    this.errGridData = [{ Message: "No Data", ErrorDetails: "", ErrorMessage: "", ErrorReason: "" }];
    this.errMsgTab = null; //variable  for kendoTabStrip loaded by content management
    this.lgrid = null; //variable  for kendoGrid
    this.mgrid = null; //variable  for kendoGrid
    this.wgrid = null; //variable  for kendoGrid
    this.egrid = null; //variable  for kendoGrid
    
    // these properties are not currently used but may be used later
    this.DataSourceName = "Logs Messages Warnings and Errors";
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    this.AddErrorMsg = "Invalid field information; cannot add new record";
    this.AddErrorTitle = "Field Data Required";
    screenLEName: null;
    IDKey: "id1234";
    ParentIDKey: "id22222";
    this.Theme = "blueopal";
    dataFields: null;  //list of fields to edit or insert
    ContainerDivID: "";
    this.kendoWindowsObjUploadEventAdded = 0;
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.PKName = "";
    this.sNGLCtrlName = ""; //variable used on page for the control like "wdgt" +  sCleanPageDetName + "Dialog"
    this.ctrlSubTypes = new GroupSubTypes();
    this.ctrlContainers = null; //variable to hold a list of all the containers that may expose CRUD  and Action operations.  the popup control will call these methods using this list
    this.readKey = 0;
        
    this.clearWdgtHTML = function () {
        //TODO: add logic to loop through other containers for future
        if (this.ctrlContainers != null) {
            $.each(this.ctrlContainers, function (index, item) {
                //currently only some objects support saving
                if (item.fieldGroupSubType === nglGroupSubTypeEnum.NGLWorkFlowOptionCtrl) {
                    var sControlVariableName = "wdgt" + item.fieldName + "Edit"
                    if (typeof (tPage[sControlVariableName]) !== 'undefined' && ngl.isObject(tPage[sControlVariableName])) {
                        tPage[sControlVariableName].clearWdgtHTML();
                    }
                }
            });
        }

    }

    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.SetFieldDefault = function (sName, sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItem = function (sID) {
        var sRet = null;
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItemByFieldID = function (sFieldID) {
        var sRet = null;
        if (!sFieldID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldID == sFieldID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.loadHTML = function () {
        var tObj = this;
        tObj.blnEditing = true; //we are always editing the popup control we do not perform CRUD operations directly
        //if (typeof (this.ctrlSubTypes) === "undefined" || !ngl.isObject(this.ctrlSubTypes)) { this.ctrlSubTypes = new GroupSubTypes(); }
        //this.ctrlSubTypes.addDefaultSubTypesIfNeeded();
        var divContainer = $("#" + this.ContainerDivID);
        divContainer.css({ position: "relative" })
        if (!divContainer) { return false; }
        // no child data is supported in this version
        return;
    };

    this.loadKendo = function () {
        // in this widget we only support the built in kendo data
        // so we just load the object using the default key ids proided
        // in Load function
        // create the tab control        
        this.errMsgTab = $("#div" + this.IDKey + "ErWaMsLgTab").kendoTabStrip().data("kendoTabStrip");

        //create the Log Grid
        this.lgrid = $("#div" + this.IDKey + "logGrid").kendoGrid({
            toolbarColumnMenu: true,
            dataSource: {
                data: this.logGridData,
                pageSize: 10
            },
            height: 400,
            scrollable: true,
            resizable: true,
            reorderable: true,
            sortable: true,
            toolbar: ["excel"], excel: { fileName: "NGLLog-" + Date.now() + ".xlsx", allPages: true },
            columns: [
                { field: "Message", title: "Message", width: "300px" },
                { field: "ErrorMessage", title: "Message", width: "100px" },
                { field: "ErrorDetails", title: "Details", width: "100px" },
                { field: "ErrorReason", title: "Reason", width: "100px" }
            ]
        }).data("kendoGrid");
        //create the Message Grid
        this.mgrid = $("#div" + this.IDKey + "msgGrid").kendoGrid({
            toolbarColumnMenu: true,
            dataSource: {
                data: this.msgGridData,
                pageSize: 10
            },
            height: 400,
            scrollable: true,
            resizable: true,
            reorderable: true,
            sortable: true,
            toolbar: ["excel"], excel: { fileName: "NGLMsg-" + Date.now() + ".xlsx", allPages: true },
            columns: [
                { field: "Message", title: "Message", width: "300px" },
                { field: "ErrorMessage", title: "Message", width: "100px" },
                { field: "ErrorDetails", title: "Details", width: "100px" },
                { field: "ErrorReason", title: "Reason", width: "100px" }
            ]
        }).data("kendoGrid");
        //create the Warning Grid
        this.wgrid = $("#div" + this.IDKey + "warnGrid").kendoGrid({
            toolbarColumnMenu: true,
            dataSource: {
                data: this.warnGridData,
                pageSize: 10
            },
            height: 400,
            scrollable: true,
            resizable: true,
            reorderable: true,
            sortable: true,
            toolbar: ["excel"], excel: { fileName: "NGLWarn-" + Date.now() + ".xlsx", allPages: true },
            columns: [
                { field: "Message", title: "Message", width: "300px" },
                { field: "ErrorMessage", title: "Message", width: "100px" },
                { field: "ErrorDetails", title: "Details", width: "100px" },
                { field: "ErrorReason", title: "Reason", width: "100px" }
            ]
        }).data("kendoGrid");
        //create the Error Grid
        this.egrid = $("#div" + this.IDKey + "errGrid").kendoGrid({
            toolbarColumnMenu: true,
            dataSource: {
                data: this.errGridData,
                pageSize: 10
            },
            height: 400,
            scrollable: true,
            resizable: true,
            reorderable: true,
            sortable: true,
            toolbar: ["excel"], excel: { fileName: "NGLWarn-" + Date.now() + ".xlsx", allPages: true },
            columns: [
                { field: "Message", title: "Message", width: "300px" },
                { field: "ErrorMessage", title: "Message", width: "100px" },
                { field: "ErrorDetails", title: "Details", width: "100px" },
                { field: "ErrorReason", title: "Reason", width: "100px" }
            ]
        }).data("kendoGrid");


        return;
       
       
    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        return ""; //not currently supported
       
    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported
    }

    this.readSuccessCallback = function (data) {

        //clear any old return data in rData
        this.rData = null;
        
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            this.clearWdgtHTML();
            this.show(data);            
        }
        
        return; 

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported
       
    }

    this.deleteSuccessCallback = function (data) {
        return ""; //not currently supported
    
    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        return ""; //not currently supported
       
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        return; //not supported
       
    }
        
    this.save = function () {
        return; //not supported       
    }

    this.show = function (data) {
        //debugger;
        var tObj = this;
        //debugger;
        var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
        if (!nglkendoWindow) {

            try {
                this.loadHTML();
                this.loadKendo();               
                var nglkendoWindow = $("#" + this.ContainerDivID).data("kendoWindow");
                if (!nglkendoWindow) {

                    this.kendoWindowsObj = $("#" + this.ContainerDivID).kendoWindow({
                        title: this.DataSourceName,
                        modal: true,
                        visible: false,
                        height: '75%',
                        width: '75%',
                        actions: [ "Minimize", "Maximize", "Close"],
                        close: function (e) { $("#" + tObj.ContainerDivID).hide(); tObj.close(e); }
                    }).data("kendoWindow");
                }

            } catch (err) {

                ngl.showErrMsg("Cannot show Window", err.message, null);

            }
           
        }
        var blnTabSelected = this.edit(data);
        if (blnTabSelected == true) {
            //this.kendoWindowsObj.refresh();
            //if (this.EditError === false) { this.kendoWindowsObj.center().open(); }
            $("#" + this.ContainerDivID).show();
            $("#" + this.ContainerDivID).data("kendoWindow").center().open();
        }

        if (typeof (this.AsyncMessagesPossible) !== 'undefined' && this.AsyncMessagesPossible == true) {
            setTimeout(function (tObj) {
                var filter = new AllFilter();
                filter.filterName = 'LoadTenderControl';
                filter.filterValue = data.Data[0];
                filter.filterFrom = '';
                filter.filterTo = ''
                filter.sortName = 'ID';
                filter.sortDirection = 'ASC';
                filter.page = 1;
                filter.skip = 0;
                filter.take = 50;

                var fValues = filter.FilterValues;
                if (typeof (fValues) === 'undefined' || ngl.isArray(fValues) === false) {
                    fValues = new Array();
                };
               

                var f = new FilterDetails();               
                f.filterID = 1;
                f.filterCaption = 'Load Tender Control';
                f.filterName = 'LoadTenderControl';
                f.filterValueFrom = this.AsyncMessageKey;
                f.filterValueTo = this.AsyncMessageKey;
                f.filterFrom = null;
                f.filterTo = null;
                fValues.push(f);

                f = new FilterDetails();
                f.filterID = 2;
                f.filterCaption = 'Load Tender Type Control';
                f.filterName = 'LoadTenderTypeControl';
                f.filterValueFrom = this.AsyncTypeKey;
                f.filterValueTo = this.AsyncTypeKey;
                f.filterFrom = null;
                f.filterTo = null;
                fValues.push(f);

                f = new FilterDetails();
                f.filterID = 3;
                f.filterCaption = 'Caller';
                f.filterName = 'Caller';
                f.filterValueFrom = 'Load Tender Results';
                f.filterValueTo = 'Load Tender Results';
                f.filterFrom = null;
                f.filterTo = null;
                fValues.push(f);

                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.filteredRead("LoadBoard/GetLoadTenderMessages", filter, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                    blnRet = false;
                    //kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                }
                //            var blnRet = oCRUDCtrl.filteredRead("P44RateQuote/GetRateRequestItems", filter, tObj, "GetRateRequestItemsCallback", "GetRateRequestItemsAjaxErrorCallback");                 

            }, 1, tObj);
        }
    }

    this.read = function (intControl) {
        return; // not supported       
    }

    //The sActionKey indicates which actions to execute
    //common keys are save and close other custom keys may be 
    //configured by each page.  Each cmPageDetail object may 
    //have an action associated with a key via the PageDetWidgetActionKey
    //and the PageDetWidgetAction. additionally each container/widget
    //may implement other logic to determine which actions to execute
    //for example the NGLWorkFlowOptionCtrl may only execute actions
    //when a switch or group is selected.
    this.executeActions = function (sActionKey) {
        var blnRet = true;
        var tObj = this;
       
        if (sActionKey === 'close') {
            this.kendoWindowsObj.close();
        }
        
        return blnRet;
    }

    this.raiseAction = function (sAction) {
        if (!sAction) { return; }
        if (isEmpty(sAction)) { return; }
        if (typeof (tPage.execActionClick) !== 'undefined' && ngl.isFunction(tPage.execActionClick)) {
            tPage.execActionClick(sAction, this.sNGLCtrlName);
        }
    }

    this.delete = function (intControl) {
        return ''; //not currently supported        
    }
    // data is passed to the show method then to edit via the var errWarnData = null; generated by the content management widget logic
    //  data is expected to be a Models.Response object typically populated like this in the controller:
    //      switchResponse.AsyncMessagesPossible = oRet.isAsyncMsgPossible();
    //      switchResponse.AsyncMessageKey = oRet.getAsyncMessageKey();
    //      switchResponse.AsyncTypeKey = oRet.getAsyncTypeKey();
    //      Utilities.addWCFMessagesToResponse(ref switchResponse, ref oRet, "Rate It");  // this formats the results 
    //      note: the above is for WCFResults data use additional functions when a different source object is required
    this.edit = function (data) {
        var tObj = this;
        var blnTabSelected = false;
        var blnLogExists = false;
        var blnMsgExists = false;
        var blnWarnExists = false;
        var blnErrExists = false;
        var sActiveTab = "#li" + tObj.IDKey + "logTab";
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            try {
               
                var blankData = [{ Message: "No Data", ErrorDetails: "", ErrorMessage: "", ErrorReason: "" }];
                if (typeof (data.AsyncMessagesPossible) !== 'undefined') {
                    tObj.AsyncMessagesPossible = data.AsyncMessagesPossible;                    
                } else {
                    tObj.AsyncMessagesPossible = false;
                }
                if (typeof (data.AsyncMessageKey) !== 'undefined') {
                    tObj.AsyncMessageKey= data.AsyncMessageKey;
                } else {
                    tObj.AsyncMessageKey = 0;
                }
                if (typeof (data.AsyncTypeKey) !== 'undefined') {
                    tObj.AsyncTypeKey = data.AsyncTypeKey;
                } else {
                    tObj.AsyncTypeKey = 0;
                }              

                if (typeof (data.Log) !== 'undefined' && ngl.isArray(data.Log) && data.Log.length > 0) {
                    blnTabSelected = true;
                    blnLogExists = true;
                    tObj.logGridData = data.Log;
                    if (ngl.isNullOrWhitespace(data.LogTitle)) {
                        $("#sp" + tObj.IDKey + "logTitle").html(data.Log.length.toString() + " Logs");
                    } else {
                        $("#sp" + tObj.IDKey + "logTitle").html(data.Log.length.toString() + ' ' + data.LogTitle);
                    }
                } else {
                    $("#sp" + tObj.IDKey + "logTitle").html("No Logs");
                    tObj.logGridData = blankData;
                }

                if (typeof (data.Message) !== 'undefined' && ngl.isArray(data.Message) && data.Message.length > 0) {
                    if (blnTabSelected != true) {
                        blnTabSelected = true;                        
                        sActiveTab = "#li" + tObj.IDKey + "msgTab";
                    }
                    blnMsgExists = true;
                    tObj.msgGridData = data.Message;
                    if (ngl.isNullOrWhitespace(data.MsgTitle)) {
                        $("#sp" + tObj.IDKey + "msgTitle").html(data.Message.length.toString() + " Messages");
                    } else {
                        $("#sp" + tObj.IDKey + "msgTitle").html(data.Message.length.toString() + ' ' + data.MsgTitle);
                    }
                } else {
                    $("#sp" + tObj.IDKey + "msgTitle").html("No Messages");
                    tObj.msgGridData = blankData;
                }

                if (typeof (data.Warn) !== 'undefined' && ngl.isArray(data.Warn) && data.Warn.length > 0) {
                    if (blnTabSelected != true) {
                        blnTabSelected = true;
                        sActiveTab = "#li" + tObj.IDKey + "warnTab";
                    }
                    blnWarnExists = true;
                    tObj.warnGridData = data.Warn;
                    if (ngl.isNullOrWhitespace(data.WarningTitle)) {
                        $("#sp" + tObj.IDKey + "warnTitle").html(data.Warn.length.toString() + " Warnings");
                    } else {
                        $("#sp" + tObj.IDKey + "warnTitle").html(data.Warn.length.toString() + ' ' + data.WarningTitle);
                    }
                } else {
                    $("#sp" + tObj.IDKey + "warnTitle").html("No Warnings");
                    tObj.warnGridData = blankData;

                }
                if (typeof (data.Err) !== 'undefined' && ngl.isArray(data.Err) && data.Err.length > 0) {
                    if (blnTabSelected != true) {
                        blnTabSelected = true;
                        sActiveTab = "#li" + tObj.IDKey + "errTab";
                    }
                    blnErrExists = true;
                    tObj.errGridData = data.Err;
                    if (ngl.isNullOrWhitespace(data.ErrTitle)) {
                        $("#sp" + tObj.IDKey + "errTitle").html(data.Err.length.toString() + " Errors");
                    } else {
                        $("#sp" + tObj.IDKey + "errTitle").html(data.Err.length.toString() + ' ' + data.ErrTitle);
                    }
                } else {
                    $("#sp" + tObj.IDKey + "errTitle").html("No Errors");
                    tObj.errGridData = blankData;
                }

                tObj.lgrid.setDataSource(tObj.logGridData);
                tObj.lgrid.refresh();
                tObj.mgrid.setDataSource(tObj.msgGridData);
                tObj.mgrid.refresh();
                tObj.wgrid.setDataSource(tObj.warnGridData);
                tObj.wgrid.refresh();
                tObj.egrid.setDataSource(tObj.errGridData);
                tObj.egrid.refresh();

                if (blnTabSelected == true) {
                    tObj.errMsgTab.activateTab($(sActiveTab));
                }
            } catch (err) {
                ngl.showErrMsg("Process Warnings Failure", err.message, tObj);
            }
        }

        return blnTabSelected; //not currently supported cannot edit       
    }

    this.close = function (e) {
        //$("#div" + this.IDKey + "wait").hide();
    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
                                    WindowPageVariable,
                                    IDKey,
                                    ParentIDKey,
                                    Theme,
                                    sNGLCtrlName) {
        //debugger;
        var tObj = this;
        this.lastErr = "";
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.ParentIDKey = ParentIDKey;
        this.Theme = Theme;
        this.kendoWindowsObj = WindowPageVariable;
        this.sNGLCtrlName = sNGLCtrlName;


    }

}



