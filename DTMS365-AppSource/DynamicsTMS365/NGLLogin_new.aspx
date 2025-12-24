<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NGLLogin_New.aspx.cs" Inherits="DynamicsTMS365.NGLLogin_New" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>DTMS Login</title>
    <style>
        @font-face {
            font-family: Arial !important;
            font-display: swap !important;
        }
    </style>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet">
    <%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>--%>

    <script src="../Scripts/kendoR32023/jquery.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
    <script>kendo.ui['Button'].fn.options['size'] = "small";</script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <%-- <script src="Scripts/NGL/v-8.5.4.006/app.js"></script>  --%>
    <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>
    <script src="Scripts/NGL/Ctrls/dataEntryForm.js"></script>
    <style>
        .background-radial-gradient {
            background-color: hsl(218, 41%, 15%);
            background-image: radial-gradient(650px circle at 0% 0%, hsl(218, 41%, 35%) 15%, hsl(218, 41%, 30%) 35%, hsl(218, 41%, 20%) 75%, hsl(218, 41%, 19%) 80%, transparent 100%), radial-gradient(1250px circle at 100% 100%, hsl(218, 41%, 45%) 15%, hsl(218, 41%, 30%) 35%, hsl(218, 41%, 20%) 75%, hsl(218, 41%, 19%) 80%, transparent 100%);
        }

        #radius-shape-1 {
            height: 220px;
            width: 220px;
            top: -60px;
            left: -130px;
            background: radial-gradient(#44006b, #ad1fff);
            overflow: hidden;
        }

        #radius-shape-2 {
            border-radius: 38% 62% 63% 37% / 70% 33% 67% 30%;
            bottom: -60px;
            right: -110px;
            width: 300px;
            height: 300px;
            background: radial-gradient(#44006b, #ad1fff);
            overflow: hidden;
        }

        .bg-glass {
            background-color: hsla(0, 0%, 100%, 0.9) !important;
            backdrop-filter: saturate(200%) blur(25px);
        }
    </style>
    <style>
        .container-login100 {
            /*width: 100%;
            min-height: 100vh;
            display: -webkit-box;
            display: -webkit-flex;
            display: -moz-box;
            display: -ms-flexbox;
            display: flex;
            flex-wrap: wrap;*/
            justify-content: center;
            align-items: center;
            padding: 15px;
            background: #9053c7;
            background: -webkit-linear-gradient(-135deg, #c850c0, #4158d0);
            background: -o-linear-gradient(-135deg, #c850c0, #4158d0);
            background: -moz-linear-gradient(-135deg, #c850c0, #4158d0);
            background: linear-gradient(-135deg, #c850c0, #4158d0);
        }

        table {
            width: 100% !important;
        }

        ::-webkit-scrollbar {
            width: 8px;
        }
        /* Track */
        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

        body {
            color: #000;
            overflow-x: hidden;
            height: 100%;
            background-color: #B0BEC5;
            background-repeat: no-repeat;
        }

        .card0 {
            box-shadow: 0px 4px 8px 0px #757575;
            border-radius: 0px;
        }

        .card2 {
            margin: 0px 40px;
        }

        .logo {
            width: auto;
            /*width: 200px;*/
            height: 100px;
            margin-top: 20px;
            margin-left: 35px;
        }

        .image {
            /*width: 360px;
            height: 280px;*/
            width: 180px;
            height: 140px;
        }

        .border-line {
            border-right: 1px solid #EEEEEE;
        }

        .facebook {
            background-color: #3b5998;
            color: #fff;
            font-size: 18px;
            padding-top: 5px;
            border-radius: 50%;
            width: 35px;
            height: 35px;
            cursor: pointer;
        }

        .twitter {
            background-color: #1DA1F2;
            color: #fff;
            font-size: 18px;
            padding-top: 5px;
            border-radius: 50%;
            width: 35px;
            height: 35px;
            cursor: pointer;
        }

        .linkedin {
            background-color: #2867B2;
            color: #fff;
            font-size: 18px;
            padding-top: 5px;
            border-radius: 50%;
            width: 35px;
            height: 35px;
            cursor: pointer;
        }

        .line {
            height: 1px;
            width: 45%;
            background-color: #E0E0E0;
            margin-top: 10px;
        }

        .or {
            width: 10%;
            font-weight: bold;
        }

        .text-sm {
            font-size: 14px !important;
        }

        ::placeholder {
            color: #BDBDBD;
            opacity: 1;
            font-weight: 300
        }

        :-ms-input-placeholder {
            color: #BDBDBD;
            font-weight: 300
        }

        ::-ms-input-placeholder {
            color: #BDBDBD;
            font-weight: 300
        }

        input, textarea {
            padding: 10px 12px 10px 12px;
            border: 1px solid lightgrey;
            border-radius: 2px;
            margin-bottom: 5px;
            margin-top: 2px;
            width: 100%;
            box-sizing: border-box;
            color: #2C3E50;
            font-size: 14px;
            letter-spacing: 1px;
        }

            input:focus, textarea:focus {
                -moz-box-shadow: none !important;
                -webkit-box-shadow: none !important;
                box-shadow: none !important;
                border: 1px solid #304FFE;
                outline-width: 0;
            }

        button:focus {
            -moz-box-shadow: none !important;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
            outline-width: 0;
        }

        a {
            color: inherit;
            cursor: pointer;
        }

        .btn-blue {
            background-color: #1A237E;
            width: 150px;
            color: #fff;
            border-radius: 2px;
        }

            .btn-blue:hover {
                color: #fff;
                background-color: #7580E8;
                cursor: pointer;
            }

        .btn-Primary {
            background-color: #E8757A;
            width: auto;
            color: #fff;
            border-radius: 2px;
        }

            .btn-Primary:hover {
                color: #fff;
                background-color: #F2999D;
                cursor: pointer;
            }

        .bg-blue {
            color: #fff;
            background-color: #1A237E;
        }

        @media screen and (max-width: 991px) {
            .logo {
                margin-left: 0px;
            }

            .image {
                width: 300px;
                height: 220px;
            }

            .border-line {
                border-right: none;
            }

            .card2 {
                border-top: 1px solid #EEEEEE !important;
                margin: 0px 15px;
            }
        }

        svg {
            width: 14px; /* Set desired width */
            height: 14px; /* Set desired height */
            display: none;
        }
    </style>
</head>
<body classname="snippet-body">
    <div id="example" class="container-login100 container-fluid px-1 px-md-5 px-lg-1 px-xl-5 py-5 mx-auto">
        <%--<div class="card card0 border-0">--%>
        <div class="w-100 px-4 py-5">
            <div class="row d-flex justify-content-center">
                <div class="col-lg-6" style="display: none !important;">
                    <div class="card1 pb-5">
                        <div class="row">
                            <%--<a id="aLogoURL" href="http://www.nextgeneration.com" target="_blank">
                                <img id="imgLogo" src="https://tms.maxximu.com/Content/Customer/DTMS_Logo.jpeg" class="logo">
                            </a>--%>
                        </div>
                        <div class="row px-3 justify-content-center mt-4 mb-5 border-line">
                            <img src="https://i.imgur.com/uNGdWHi.png" class="image">
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="card2 card border-0 px-4 py-5">
                        <div class="row">
                            <a id="aLogoURL" href="http://www.nextgeneration.com" target="_blank">
                                <img id="imgLogo" src="https://tms.maxximu.com/Content/Customer/DTMS_Logo.jpeg" class="logo">
                            </a>
                        </div>
                        <div class="row px-3">
                            <label class="mb-1 text-center">
                                <h6 class="mb-0 text-sm"><strong>Dynamics TMS&trade; Account</strong></h6>
                            </label>
                            <label class="mb-1 text-center">
                                <h6 class="mb-0 text-sm" id="WelcomeMessage">Please sign in using one of the following options.</h6>
                            </label>
                        </div>
                        <% Response.Write(PageDataTableHTML); %>
                        <%--<div class="row px-3">
                            <label class="mb-1">
                                <h6 class="mb-0 text-sm">User Name</h6>
                            </label>
                            <input class="mb-4" type="text" name="email" placeholder="Enter a valid email address">
                        </div>
                        <div class="row px-3">
                            <label class="mb-1">
                                <h6 class="mb-0 text-sm">Password</h6>
                            </label>
                            <input type="password" name="password" placeholder="Enter password">
                        </div>--%>
                        <div class="row px-3 mb-4">
                            <div class="custom-control custom-checkbox custom-control-inline">
                                <input id="chk1" type="checkbox" name="chk" class="custom-control-input">
                                <label for="chk1" class="custom-control-label text-sm">Remember me</label>
                            </div>
                            <a id="lnkForgotPassword" href="ForgotPassword.aspx" class="ml-auto mb-0 text-sm">Forgot Password?</a>
                        </div>
                        <div class="row mb-3 px-3">
                            <%--<button type="submit" class="btn btn-blue text-center">Login</button>--%>
                            <a id='NGLSignIn' class="btn btn-blue text-center" onclick='NGLSignIn();' href='#' style='margin: 6px; vertical-align: top;'>
                                <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                <span id='NGLsignInText' style='vertical-align: middle;'>Sign In</span>
                            </a>
                        </div>
                        <div class="row px-3 mb-4">
                            <div class="line"></div>
                            <small class="or text-center">Or</small>
                            <div class="line"></div>
                        </div>
                        <div class="row mb-4 px-3">
                            <%--<h6 class="mb-0 mr-4 mt-2">Sign in with Your Microsoft Account</h6>--%>
                            <a id='AADSignIn' class='btn btn-Primary text-center' onclick='callerSignIn();' href='#' style='margin: 6px; vertical-align: top;'>
                                <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                <span id='AADsignInText' style='vertical-align: middle;'>Sign in with Your Microsoft Account</span>
                            </a>
                            <%--<div class="facebook text-center mr-3">
                                <div class="fa fa-facebook"></div>
                            </div>
                            <div class="twitter text-center mr-3">
                                <div class="fa fa-twitter"></div>
                            </div>
                            <div class="linkedin text-center mr-3">
                                <div class="fa fa-linkedin"></div>
                            </div>--%>
                        </div>
                        <div class="row px-3 mb-4">
                            <div class="line"></div>
                            <small class="or text-center">Or</small>
                            <div class="line"></div>
                        </div>

                        <div id="divWindowsAccount" class="row mb-4 px-3">
                            <small class="font-weight-bold">Don't have an account? <a id="lnkRequestAccount" href="RequestAnAccount.aspx" class="text-danger ">Register</a></small>&nbsp;&nbsp;|&nbsp;&nbsp;
                            <p>
                                <a id="lnkBackToPublicSite" class="btn btn-info text-sm" href="<% Response.Write(HomeTabHrefURL); %>" target="_blank">
                                    <%--<img id="imgBackToPublicSite" src="Content/NGL/BackToPublicSite.gif" style="border: 0;" />--%>
                                    Go Back To Public Site
                                </a>
                            </p>
                        </div>
                        <div id="divFreeTrial" class="row mb-4 px-3">
                            <h4 class="mb-0 text-sm">Would you like to know more?</h4>
                            &nbsp;&nbsp;
                            <a id='freeSignIn' class='btn btn-warning text-sm' onclick="window.open('https://dynamicstms.com/', '_blank');">
                                <span class='k-icon k-i-user' style='vertical-align: middle;'></span>
                                <span id='freesignInText' style='vertical-align: middle;'>Get More Information</span>
                            </a>
                            <input id="signInText" type="hidden" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="bg-blue py-4">
                <div class="row px-3">
                    <%--<small class="ml-4 ml-sm-5 mb-2">Copyright © 2019. All rights reserved.</small>--%>
                    <small class="ml-4 ml-sm-5 mb-2"><a id="aFooterURL" href="<% Response.Write(HomeTabHrefURL); %>" target="_blank"><% Response.Write(PageFooterHTML); %> </a></small>
                    <div class="social-contact ml-4 ml-sm-auto">
                        <span class="fa fa-facebook mr-4 text-sm"></span>
                        <span class="fa fa-google-plus mr-4 text-sm"></span>
                        <span class="fa fa-linkedin mr-4 text-sm"></span>
                        <span class="fa fa-twitter mr-4 mr-sm-5 text-sm"></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <% Response.Write(AuthLoginNotificationHTML); %>
    <%--<script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>--%>
    <script type="text/javascript">
        var myLink = document.querySelectorAll('a[href="#"]');
        myLink.forEach(function (link) {
            link.addEventListener('click', function (e) {
                e.preventDefault();
            });
        });
    </script>
    <script>
        //debugger;
        //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName();
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        var caller = '<%=Caller%>';
        //loadAuthContext();
        loadCallerAuthContext();


        function loadDataEntryCtrl() {
            $('#userDataEntry').load('Views/dataEntrtyForm.html');
            //$errorMessage.empty();
            var ctrl = dataEntryForm;

            if (!ctrl)
                return;
            var dataRows = [];
            var dataRow = new dataEntryRow();
            dataRow.caption = "User Name:"
            dataRow.id = "txtNGLUserName";
            dataRow.controltype = "0";
            dataRow.metaData = "";
            dataRows.push(dataRow)
            var dataRowP = new dataEntryRow();
            dataRowP.caption = "Password:"
            dataRowP.id = "txtNGLPass";
            dataRowP.controltype = "0";
            dataRowP.metaData = "";
            dataRows.push(dataRowP)
            var html = $('#userDataEntry');
            //debugger;
            //   .find(".data-container").empty();
            //    $panel.html($html.html());
            //    ctrl.postProcess(html,dataRows);
            ctrl.postProcess(html, dataRows);
            $('#userDataEntry').show();
            // Load View HTML
            //$.ajax({
            //    type: "GET",
            //    url: "Views/dataEntrtyForm.html",
            //    dataType: "html",
            //}).done(function (html) {

            //    // Show HTML Skeleton (Without Data)
            //    var $html = $(html);
            //    $html.find(".data-container").empty();
            //    $panel.html($html.html());
            //    ctrl.postProcess(html,dataRows);

            //}).fail(function () {
            //    $errorMessage.html('Error loading page.');
            //}).always(function () {

            //});
        }

        // Modified by RHR for v-8.5.4.005 on 01/29/2024 moved security information fro filter into header
        //     filter is no longer used.
        function NGLSignIn() {

            //debugger;
            //var sfilter = new NGLClass14();
            var sfilter = new NGLClass14();
            sfilter.NGLvar1455 = $("#txtNGLUserName").data("kendoMaskedTextBox").value();
            sfilter.NGLvar1450 = $("#txtNGLPass").data("kendoMaskedTextBox").value();

            //data: { filter: JSON.stringify(sfilter) },
            $.ajax({
                async: false,
                type: "GET",
                url: "api/SSOA/GetNGLSSOAToken",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { filter: '' },
                headers: { "NGLClass14": JSON.stringify(sfilter) },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        //debugger;
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("User Login Validation Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        dataitem = data.Data[0];
                                        if ('UserName' in dataitem && typeof (dataitem.UserName) !== 'undefined' && dataitem.UserName !== null) {
                                            //username is required and cannot be null
                                            localStorage.NGLvar1455 = dataitem.UserName;
                                        } else {
                                            blnSuccess = false;
                                            strValidationMsg = "The user name or password is not valid.  Please try again";
                                        }
                                        if (blnSuccess === true && 'UserSecurityControl' in dataitem) { localStorage.NGLvar1452 = dataitem.UserSecurityControl; }
                                        if (blnSuccess === true && 'USATUserID' in dataitem) { localStorage.NGLvar1451 = dataitem.USATUserID; }
                                        if (blnSuccess === true && 'USATToken' in dataitem) { localStorage.NGLvar1454 = dataitem.USATToken; }
                                        if (blnSuccess === true && 'SSOAControl' in dataitem) { localStorage.NGLvar1472 = dataitem.SSOAControl; }
                                        if (blnSuccess === true && 'SSOAUserEmail' in dataitem) { localStorage.NGLvar1458 = dataitem.SSOAUserEmail; }
                                        localStorage.NGLvar1474 = ""; //data.Data[0].NGLvar1474 aka JWT Token;
                                        if (blnSuccess === true && 'UserFriendlyName' in dataitem) { localStorage.NGLvar1457 = dataitem.UserFriendlyName; }
                                        //for now we do not do anything with the read method
                                        //this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                                        if (blnSuccess === true) {
                                            var divWelcome = document.getElementById('WelcomeMessage');
                                            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                                divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
                                            }
                                            var uc = localStorage.NGLvar1452;
                                            if ('CatControl' in dataitem && dataitem.CatControl == '2') {
                                                caller = "Default.aspx";
                                                //document.location = "../Default?uc=" + uc;
                                                //document.location = "../Login?uc=" + uc + "&caller=" + caller;
                                            }
                                            //else {
                                            //    document.location = caller + "?uc=" + uc;
                                            //}
                                            document.location = "../Login?uc=" + uc + "&caller=" + caller;


                                        }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Account information not found"; }
                            ngl.showErrMsg("User Login Validation Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Authentication Failure");
                    ngl.showErrMsg("User Login Validation", sMsg, null);
                }
            });


        }

        //validateLogin(caller);
        $(document).ready(function () {
            //debugger;
            //
            control = <%=UserControl%>;
            //loadDataEntryCtrl();
            //if(blnReload == true ){
            //    var uc = localStorage.NGLvar1452;
            //    document.location = caller + "?uc=" + uc;
            //    return;
            //}

            //Hide the WindowsAccount and FreeTrial buttons if this is not AppSource
            // Modified by RHR for v-8.5.3.007 we only hide the free trial button when is IsAppSource is true
            $("#divWindowsAccount").show();
            var blnIsAppource = '<%=ConfigurationManager.AppSettings["IsAppSource"]%>';
            if (blnIsAppource.toLowerCase() === "true") {
                //$("#divWindowsAccount").show();
                $("#divFreeTrial").show();
            }
            else {
                //$("#divWindowsAccount").hide();
                $("#divFreeTrial").hide();
            }

            //Code to change logo  and logo url based on web config
            document.getElementById('imgLogo').src = '<%=ConfigurationManager.AppSettings["HomeTabLogo"]%>';
            document.getElementById('aLogoURL').setAttribute('href','<%=ConfigurationManager.AppSettings["HomeTabHrefURL"]%>');


            $("#txtNGLUserName").kendoMaskedTextBox();
            $("#txtNGLPass").kendoMaskedTextBox();


            $("#btnAADSignIn").kendoButton({
                click: function (e) {
                    signIn();
                }

            });


        });


    </script>
</body>
</html>
