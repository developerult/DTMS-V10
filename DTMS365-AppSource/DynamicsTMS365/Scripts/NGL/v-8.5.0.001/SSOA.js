//configuration
// a WelcomeMessage div tag must exist in the html

//ADAL error codes
//invalid_request	Protocol error, such as a missing required parameter.	Fix and resubmit the request. This is a development error, and is typically caught during initial testing.
//unauthorized_client	The client application is not permitted to request an authorization code.	This usually occurs when the client application is not registered in Azure AD or is not added to the user's Azure AD tenant. The application can prompt the user with instruction for installing the application and adding it to Azure AD.
//access_denied	Resource owner denied consent	The client application can notify the user that it cannot proceed unless the user consents.
//unsupported_response_type	The authorization server does not support the response type in the request.	Fix and resubmit the request. This is a development error, and is typically caught during initial testing.
//server_error	The server encountered an unexpected error.	Retry the request. These errors can result from temporary conditions. The client application might explain to the user that its response is delayed due to a temporary error.
//temporarily_unavailable	The server is temporarily too busy to handle the request.	Retry the request. The client application might explain to the user that its response is delayed due to a temporary condition.
//invalid_resource	The target resource is invalid because it does not exist, Azure AD cannot find it, or it is not correctly configured.	This indicates the resource, if it exists, has not been configured in the tenant. The application can prompt the user with instruction for installing the application and adding it to Azure AD.

//ADAL JWT Token Claims from id_token
// Sample:
//"aud": "2d4d11a2-f814-46a7-890a-274a72a7309e",
//"iss": "https://sts.windows.net/7fe81447-da57-4385-becb-6de57f21477e/",
//"iat": 1388440863,
//"nbf": 1388440863,
//"exp": 1388444763,
//"ver": "1.0",
//"tid": "7fe81447-da57-4385-becb-6de57f21477e",
//"oid": "68389ae2-62fa-4b18-91fe-53dd109d74f5",
//"upn": "frank@contoso.com",
//"unique_name": "frank@contoso.com",
//"sub": "JWvYdCWPhhlpS1Zsf7yYUxShUwtUm5yzPmw_-jX3fHY",
//"family_name": "Miller",
//"given_name": "Frank"

// Specifications
//aud	Audience of the token. When the token is issued to a client application, the audience is the client_id of the client.
//exp	Expiration time. The time when the token expires. For the token to be valid, the current date/time must be less than or equal to the exp value. The time is represented as the number of seconds from January 1, 1970 (1970-01-01T0:0:0Z) UTC until the time the token was issued.
//family_name	User’s last name or surname. The application can display this value.
//given_name	User’s first name. The application can display this value.
//iat	Issued at time. The time when the JWT was issued. The time is represented as the number of seconds from January 1, 1970 (1970-01-01T0:0:0Z) UTC until the time the token was issued.
//iss	Identifies the token issuer
//nbf	Not before time. The time when the token becomes effective. For the token to be valid, the current date/time must be greater than or equal to the Nbf value. The time is represented as the number of seconds from January 1, 1970 (1970-01-01T0:0:0Z) UTC until the time the token was issued.
//oid	Object identifier (ID) of the user object in Azure AD.
//sub	Token subject identifier. This is a persistent and immutable identifier for the user that the token describes. Use this value in caching logic.
//tid	Tenant identifier (ID) of the Azure AD tenant that issued the token.
//unique_name	A unique identifier for that can be displayed to the user. This is usually a user principal name (UPN).
//upn	User principal name of the user.
//ver	Version. The version of the JWT token, typically 1.0.
// Additional fields available but not doucmenterd
//email
//idp  provider like live.com
//ipaddr  local machine ip address
//name ADAL formula using the upn name if available 
//nOnce ?
//platf ?
//username ?
var opostLogoutRedirectUri = window.location.href;
var oredirectUri = window.location.href;
var oidaClient = 'ab8def6a-9066-4657-8118-dcb775e48a92';
var oAuth2instasnce = 'https://login.microsoftonline.com/';
var oAuth2tenant = 'common';
var ADAL = null;
var caller = "Default.aspx"

function loadCallerAuthContext() {
    //alert("loadAuthContext");
    //alert(oredirectUri);
    ADAL = new AuthenticationContext({
        instance: oAuth2instasnce,
        tenant: oAuth2tenant, // 'rramseynextgeneration.onmicrosoft.com', // 'common', //COMMON OR YOUR TENANT ID

        //clientId: '4f5b2bae-3878-4b43-b4a8-50059ceb5e73', //This is Laurens client ID port 3001
        //clientId: 'ab8def6a-9066-4657-8118-dcb775e48a92', //This is Robs client ID --  rramseynextgeneration.onmicrosoft.com dynamicsTMS365Test
        //clientId: '63808392-f86b-449a-a1b1-ecadfe1476bf', //This is NGL client ID -- DynamicsTMS365Beta
        ////clientId: '789cd19a-dfa0-4a00-bf4b-0ac16ac668c3', //DynamicsTMS365Alpha
        //clientId: '5152788b-b49c-44f6-9718-c8790a1b757f',  //-- Identity APP for quick start port 8050
        //clientId: '8c915428-82cc-4e6f-bcd1-f5c0fa28e7fb',  //-- DynamicsTMS365Dev  identity port 44320
        clientId: oidaClient,
        redirectUri: oredirectUri, // 'http://localhost:44320/Tariff', //This is your redirect URI

        //   <add key="ida:ClientId" value="ab8def6a-9066-4657-8118-dcb775e48a92" />
        //<add key="ida:Tenant" value="rramseynextgeneration.onmicrosoft.com" />
        //<add key="ida:AADInstance" value="https://login.microsoftonline.com/{0}" />
        postLogoutRedirectUri: opostLogoutRedirectUri, //'http://localhost:44320/'
        cacheLocation: 'localStorage',
        callback: callerSignIn,
        popUp: true
        //popUp: false
    });

}

function loadAuthContext() {
    //alert("loadAuthContext");
//alert(oredirectUri);
ADAL = new AuthenticationContext({
    instance: oAuth2instasnce,
    tenant: oAuth2tenant, // 'rramseynextgeneration.onmicrosoft.com', // 'common', //COMMON OR YOUR TENANT ID

    //clientId: '4f5b2bae-3878-4b43-b4a8-50059ceb5e73', //This is Laurens client ID port 3001
    //clientId: 'ab8def6a-9066-4657-8118-dcb775e48a92', //This is Robs client ID --  rramseynextgeneration.onmicrosoft.com dynamicsTMS365Test
    //clientId: '63808392-f86b-449a-a1b1-ecadfe1476bf', //This is NGL client ID -- DynamicsTMS365Beta
    ////clientId: '789cd19a-dfa0-4a00-bf4b-0ac16ac668c3', //DynamicsTMS365Alpha
    //clientId: '5152788b-b49c-44f6-9718-c8790a1b757f',  //-- Identity APP for quick start port 8050
    //clientId: '8c915428-82cc-4e6f-bcd1-f5c0fa28e7fb',  //-- DynamicsTMS365Dev  identity port 44320
    clientId: oidaClient,
    redirectUri: oredirectUri, // 'http://localhost:44320/Tariff', //This is your redirect URI

    //   <add key="ida:ClientId" value="ab8def6a-9066-4657-8118-dcb775e48a92" />
    //<add key="ida:Tenant" value="rramseynextgeneration.onmicrosoft.com" />
    //<add key="ida:AADInstance" value="https://login.microsoftonline.com/{0}" />
    postLogoutRedirectUri: opostLogoutRedirectUri, //'http://localhost:44320/'
    cacheLocation: 'localStorage',
    callback: userSignedIn
    //popUp: true
    //popUp: false
});

}


function signInOut(){
    if (localStorage.SignedIn == "t") {
        signOut();
    } else {
        document.location = "../NGLLogin.aspx?caller=" +getCurentFileName();
        //signIn();
    }
   
}

function callerSignIn(err, cToken) {
    //alert("signIn");
    //debugger;
        if (!err) {
            //var cToken = ADAL.getCachedToken(ADAL.config.clientId);

           
            if (cToken != null) {
                var user = ADAL.getCachedUser();
                if (user != null) {
                    //var divWelcome = document.getElementById('WelcomeMessage');
                    //var sName = user.profile.name;
                    //if (sName.trim() === '') {
                    //    sName = user.profile.given_name + ' ' + user.profile.family_name;
                    //}
                    //divWelcome.innerHTML = "Welcome " + sName;
                    //localStorage.SignedIn = "t";
                    //var signInMsg = document.getElementById('signInText');
                    //signInMsg.innerHTML = "Sign Out";
                    //hideloginNotification();
                    var blnLoggedIn = postSSOResults(user, cToken);
                    if (blnLoggedIn === true) {
                        var uc = localStorage.NGLvar1452;
                        document.location = "../" + caller + "?uc=" + uc;
                    }
                    
                } else {
                    //alert("signIn --> acquireToken");
                    ADAL.acquireToken(ADAL.config.clientId, validateCallerToken);
                }
            } else {

                control = 0;
                //alert("signIn --> login");
                ADAL.login();
            }
        }
        else {
            showError(err);
            //console.error("error: " + err);
        }
}


function signIn() {
    //alert("signIn");
    var cToken = ADAL.getCachedToken(ADAL.config.clientId);


    if (cToken != null) {
        var user = ADAL.getCachedUser();
        if (user != null) {
            var divWelcome = document.getElementById('WelcomeMessage');
            var sName = user.profile.name;
            if (sName.trim() === '') {
                sName = user.profile.given_name + ' ' + user.profile.family_name;
            }
            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Welcome " + sName; }
            localStorage.SignedIn = "t";
            var signInMsg = document.getElementById('signInText');
            if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign Out"; }
            //hideloginNotification();
            postSSOResults(user, cToken);

        } else {
            //alert("signIn --> acquireToken");
            ADAL.acquireToken(ADAL.config.clientId, validateToken);
        }
    } else {

        control = 0;
        //alert("signIn --> login");
        ADAL.login();
    }
}

function signOut() {
    if (localStorage.NGLvar1472 === "1") {
        localStorage.clear();

        //redirect to the home page
        //document.location = caller;
        document.location.assign("../LogOut.aspx");
              
        var signInMsg = document.getElementById('signInText');
        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign In"; }
        //showloginNotification("Not Authorized", "You must login with your NGL Account")
    }
    else {
        ADAL.logOut();
        //localStorage.SignedIn = "f";
        localStorage.clear();
        var signInMsg = document.getElementById('signInText');
        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign In"; }
        showloginNotification("Not Authorized", "Please sign in to continue")
    }
}


function postLoginResults(user, token) {
    var results = new SSOResults();
    results.PrimaryComputer = true; //on new users we ask if this is the primary computer and if this is a private or public computer
    results.PublicComputer = false; //on new users we ask if this is a public/private computer and if this is a private or public computer
    results.SSOAControl = 0; //zero for new users; system will look up control with SSOAName on new users
    results.SSOAName = "Microsoft"; //use Web config SSOADefaultName for new users
    results.SSOAClientID = oidaClient; //use Web config idaClientId for new users; empty when using NGL Authentication
    results.SSOALoginURL = oAuth2instasnce; //use Web config idaInstance for new users; empty when using NGL Authentication
    results.SSOARedirectURL = oredirectUri; //use Web config WebBaseURI for new users; empty when using NGL Authentication
    results.SSOAClientSecret = ""; //empty when using NGL Authentication
    results.SSOAAuthCode = oAuth2tenant; //use tenant use Web config idaTenant for new users;  default = common
    results.UserSecurityControl = 0; //maps to tblUserSecurity.UserSecurityControl if zero this is a new user and a new record needs to be created 
    results.UserName = "";  //maps to tblUserSecurity.UserName for new users this maps to HttpContext.Current.User.Identity if this is the primary computer and it is private not public 
    results.UserLastName = user.profile.family_name; //maps to tblUserSecurity.UserLastName from user.profile.family_name on new users
    results.UserFirstName = user.profile.given_name; //maps to tblUserSecurity.UserFirstName from user.profile.given_name on new users and also maps to and tblUserSecurity.UserFriendlyName on new users
    results.USATUserID = user.profile.unique_name; //maps to tblUserSecurityAccessToken.USATUserID from user.profile.unique_name on new users if PrimaryComputer = false or PublicComputer = true maps to tblUserSecurity.UserName
    results.USATToken = token; //maps to tblUserSecurityAccessToken.USATToken from JWT token retrieved via validateUser ADAL.getCachedToken
    results.SSOAUserEmail = user.profile.email; //maps to tblUserSecurity.UserEmail from user.profile.email on new users
    results.SSOAExpires = user.profile.exp; //maps to user.profile.exp: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
    results.SSOAIssuedAtTime = user.profile.iat;
    //Added By LVV 7/31/17 for v-8.0 TMS365
    //These items will be populatedby the system so default values are fine for now
    results.IsUserCarrier = false; 
    results.UserCarrierControl = 0;
    results.UserCarrierContControl = 0;
    results.UserLEControl = 0;
    results.UserFriendlyName = "";

    if (localStorage.NGLvar1451 === results.USATUserID) {
        results.UserSecurityControl = localStorage.NGLvar1452;
        results.UserName = localStorage.NGLvar1455;
    }
    $.ajax({
        async: false,
        type: "POST",
        url: "api/SSOA/PostSSOResults",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: { filter: JSON.stringify(results) },
        success: function (data) {
            localStorage.NGLvar1455 = data.Data[0].UserName;
            localStorage.NGLvar1452 = data.Data[0].UserSecurityControl;
            localStorage.NGLvar1451 = data.Data[0].USATUserID;
            localStorage.NGLvar1454 = data.Data[0].USATToken;
            localStorage.NGLvar1472 = data.Data[0].SSOAControl;
            localStorage.NGLvar1458 = data.Data[0].SSOAUserEmail;
            localStorage.NGLvar1474 = ""; // aka JWT Token;
            localStorage.NGLvar1457 = data.Data[0].UserFriendlyName;
            
            var divWelcome = document.getElementById('WelcomeMessage');
            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1457;
            }
            localStorage.SignedIn = "t";
            return true;
        },
        error: function (xhr, textStatus, error) {
            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Authentication Failure");
            ngl.showErrMsg("Process Login Results", sMsg, null);      
            localStorage.SignedIn = "f";
            return false;
        }
    });
}


function postSSOResults(user, token) {
    var blnRet = false;
    //alert("postSSOResults");
    var results = new SSOResults();
    results.PrimaryComputer = true; //on new users we ask if this is the primary computer and if this is a private or public computer
    results.PublicComputer = false; //on new users we ask if this is a public/private computer and if this is a private or public computer
    results.SSOAControl = 0; //zero for new users; system will look up control with SSOAName on new users
    results.SSOAName = "Microsoft"; //use Web config SSOADefaultName for new users
    results.SSOAClientID = oidaClient; //use Web config idaClientId for new users; empty when using NGL Authentication
    results.SSOALoginURL = oAuth2instasnce; //use Web config idaInstance for new users; empty when using NGL Authentication
    results.SSOARedirectURL = oredirectUri; //use Web config WebBaseURI for new users; empty when using NGL Authentication
    results.SSOAClientSecret = ""; //empty when using NGL Authentication
    results.SSOAAuthCode = oAuth2tenant; //use tenant use Web config idaTenant for new users;  default = common
    results.UserSecurityControl = 0; //maps to tblUserSecurity.UserSecurityControl if zero this is a new user and a new record needs to be created 
    results.UserName = "";  //maps to tblUserSecurity.UserName for new users this maps to HttpContext.Current.User.Identity if this is the primary computer and it is private not public 
    results.UserLastName = user.profile.family_name; //maps to tblUserSecurity.UserLastName from user.profile.family_name on new users
    results.UserFirstName = user.profile.given_name; //maps to tblUserSecurity.UserFirstName from user.profile.given_name on new users and also maps to and tblUserSecurity.UserFriendlyName on new users
    results.USATUserID = user.profile.unique_name; //maps to tblUserSecurityAccessToken.USATUserID from user.profile.unique_name on new users if PrimaryComputer = false or PublicComputer = true maps to tblUserSecurity.UserName
    results.USATToken = token; //maps to tblUserSecurityAccessToken.USATToken from JWT token retrieved via validateUser ADAL.getCachedToken
    results.SSOAUserEmail = user.profile.email; //maps to tblUserSecurity.UserEmail from user.profile.email on new users
    results.SSOAExpires = user.profile.exp; //maps to user.profile.exp: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
    results.SSOAIssuedAtTime = user.profile.iat;

    //Added By LVV 7/31/17 for v-8.0 TMS365
    //These items will be populatedby the system so default values are fine for now
    results.IsUserCarrier = false;
    results.UserCarrierControl = 0;
    results.UserCarrierContControl = 0;
    results.UserLEControl = 0;
    results.UserFriendlyName = "";

    if (localStorage.NGLvar1451 === results.USATUserID) {
        results.UserSecurityControl = localStorage.NGLvar1452;
        results.UserName = localStorage.NGLvar1455;
    }
    try {
        $.ajax({
            async: false,
            type: "POST",
            url: "api/SSOA/PostSSOResults",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(results),
            success: function (data) {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    var blnErrorHandled = false;
                    if (typeof (data.Errors) !== 'undefined' && data.Errors !== null) {
                        blnErrorHandled = true;
                        ngl.showErrMsg("Post Single Sign On Results Failure", data.Errors, null);
                    }
                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0) {
                            localStorage.NGLvar1455 = data.Data[0].UserName;//
                            localStorage.NGLvar1452 = data.Data[0].UserSecurityControl;//
                            localStorage.NGLvar1451 = data.Data[0].USATUserID;//
                            localStorage.NGLvar1454 = data.Data[0].USATToken; //
                            localStorage.NGLvar1472 = data.Data[0].SSOAControl;//
                            localStorage.NGLvar1458 = data.Data[0].SSOAUserEmail;//
                            localStorage.NGLvar1474 = ""; // aka JWT Token;
                            localStorage.NGLvar1457 = data.Data[0].UserFriendlyName;//
                            
                            var divWelcome = document.getElementById('WelcomeMessage');
                            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1457;
                            }

                            localStorage.SignedIn = "t";

                            var signInMsg = document.getElementById('signInText');
                            if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {
                                signInMsg.innerHTML = "Sign Out";
                            }
                            hideloginNotification();
                            blnRet = true;
                        }
                        else {
                            if (blnErrorHandled === false) {
                                ngl.showErrMsg("Post Single Sign On Results Failure", "No data available on server.", null);
                            }
                            
                        }
                    }
                }
                else
                {
                    ngl.showErrMsg("Post Single Sign On Results Failure", "No data returned from on server.", null);
                }                                       
              
            },
            error: function (xhr, textStatus, error) {
                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                ngl.showErrMsg("Read Editor Content", sMsg, null);
            }
        });
    } catch (err) {
        ngl.showErrMsg("Post Single Sign On Results Failure", err.message, null);
    }
    return blnRet;
}


function postSSOResultsSilent(user, token) {
    var strRet = "";
    var results = new SSOResults();
    results.PrimaryComputer = true; //on new users we ask if this is the primary computer and if this is a private or public computer
    results.PublicComputer = false; //on new users we ask if this is a public/private computer and if this is a private or public computer
    results.SSOAControl = 0; //zero for new users; system will look up control with SSOAName on new users
    results.SSOAName = "Microsoft"; //use Web config SSOADefaultName for new users
    results.SSOAClientID = oidaClient; //use Web config idaClientId for new users; empty when using NGL Authentication
    results.SSOALoginURL = oAuth2instasnce; //use Web config idaInstance for new users; empty when using NGL Authentication
    results.SSOARedirectURL = oredirectUri; //use Web config WebBaseURI for new users; empty when using NGL Authentication
    results.SSOAClientSecret = ""; //empty when using NGL Authentication
    results.SSOAAuthCode = oAuth2tenant; //use tenant use Web config idaTenant for new users;  default = common
    results.UserSecurityControl = 0; //maps to tblUserSecurity.UserSecurityControl if zero this is a new user and a new record needs to be created 
    results.UserName = "";  //maps to tblUserSecurity.UserName for new users this maps to HttpContext.Current.User.Identity if this is the primary computer and it is private not public 
    results.UserLastName = user.profile.family_name; //maps to tblUserSecurity.UserLastName from user.profile.family_name on new users
    results.UserFirstName = user.profile.given_name; //maps to tblUserSecurity.UserFirstName from user.profile.given_name on new users and also maps to and tblUserSecurity.UserFriendlyName on new users
    results.USATUserID = user.profile.unique_name; //maps to tblUserSecurityAccessToken.USATUserID from user.profile.unique_name on new users if PrimaryComputer = false or PublicComputer = true maps to tblUserSecurity.UserName
    results.USATToken = token; //maps to tblUserSecurityAccessToken.USATToken from JWT token retrieved via validateUser ADAL.getCachedToken
    results.SSOAUserEmail = user.profile.email; //maps to tblUserSecurity.UserEmail from user.profile.email on new users
    results.SSOAExpires = user.profile.exp; //maps to user.profile.exp: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
    results.SSOAIssuedAtTime = user.profile.iat;

    //Added By LVV 7/31/17 for v-8.0 TMS365
    //These items will be populatedby the system so default values are fine for now
    results.IsUserCarrier = false;
    results.UserCarrierControl = 0;
    results.UserCarrierContControl = 0;
    results.UserLEControl = 0;
    results.UserFriendlyName = "";

    if (localStorage.NGLvar1451 === results.USATUserID) {
        results.UserSecurityControl = localStorage.NGLvar1452;
        results.UserName = localStorage.NGLvar1455;
    }
    try {
        $.ajax({
            async: false,
            type: "POST",
            url: "api/SSOA/PostSSOResults",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(results),
            success: function (data) {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    var blnErrorHandled = false;
                    if (typeof (data.Errors) !== 'undefined' && data.Errors !== null) {
                        blnErrorHandled = true;
                        strRet = "Post Single Sign On Results Failure: " + data.Errors;
                    }
                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0) {
                            localStorage.SignedIn = "t";                         
                            localStorage.NGLvar1455 = data.Data[0].UserName;
                            localStorage.NGLvar1452 = data.Data[0].UserSecurityControl;
                            localStorage.NGLvar1451 = data.Data[0].USATUserID;
                            localStorage.NGLvar1454 = data.Data[0].USATToken;
                            localStorage.NGLvar1472 = data.Data[0].SSOAControl;
                            localStorage.NGLvar1458 = data.Data[0].SSOAUserEmail;
                            localStorage.NGLvar1474 = ""; // aka JWT Token;
                            localStorage.NGLvar1457 = data.Data[0].UserFriendlyName;
                        }
                        else {
                            if (blnErrorHandled === false) {
                                strRet = "Post Single Sign On Results Failure: No data available on server.";
                            }

                        }
                    }
                }
                else {
                    strRet = "Post Single Sign On Results Failure: No data returned from on server.";
                }

            },
            error: function (xhr, textStatus, error) {
                strRet = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");               
            }
        });
    } catch (err) {
        strRet = "Post Single Sign On Results Failure: " + err.message;
    }
    return strRet;
}

function userSignedIn(err, cToken) {
    alert("userSignedIn")
    //console.log('userSignedIn called');
    //debugger;
    if (!err) {
        if (typeof (cToken) != 'undefined' && cToken != null) {
            var user = ADAL.getCachedUser();
            if (user != null) {                
                var blnLoggedIn = postSSOResults(user, cToken);                
                if (blnLoggedIn === true) {
                    var uc = localStorage.NGLvar1452;
                    document.location = oredirectUri + "?uc=" + uc;
                } 
            } else {
                //alert("signIn --> acquireToken");
                ADAL.acquireToken(ADAL.config.clientId, validateCallerToken);
            }
        } else {

            control = 0;
            //alert("signIn --> login");
            ADAL.login();
        }
    }
    else {
        ngl.showErrMsg("Authentication Failed:",err,null);
        //console.error("error: " + err);
    }
}


function userSignedInOld(err, token) {
    //console.log('userSignedIn called');
    if (!err) {
        //console.log("token: " + token);
        showWelcomeMessage();

        //ADAL.acquireToken(ADAL.config.clientId, validateToken);
    }
    else {
        showError(err);
        //console.error("error: " + err);
    }
}

//check if an existing token is available and refresh the JWT user info
//Call the SSOA REST Service to save the Token info on the server
function isLoggedIn(strMsg) {
    var blnRet = false;
    //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent NGL token logic
    var cToken = ADAL.getCachedToken(ADAL.config.clientId);

    if (cToken != null) {
        var user = ADAL.getCachedUser();
        if (user != null) {
            blnRet = true;
            strMsg = postSSOResultsSilent(user, cToken);            
        } 
    } else {
        signIn();       
    }
    return blnRet;
}

//check if an existing token is available and refresh the JWT user info
//Call the SSOA REST Service to save the Token info on the server
//optional parameter added to suppress messages used for pages where authentication is not required
// modified by RHR for v-8.4.0.003 added optional parameter for sCaller
//  we use getCurentFileName as the default
function validateUser(suppressMsgs, ucRequired, sCaller) {
    //debugger;
    var ssoaControl = localStorage.NGLvar1472;
    if (typeof (ssoaControl) === 'undefined' || ssoaControl === null) {
        //debugger;
        document.location = "../NGLLogin.aspx?caller=" +sCaller;
        return false;
    }
    if (!ngl.stringHasValue(sCaller)) {
        sCaller = getCurentFileName();
        if (sCaller == "NGLLogin" || sCaller == "NGLLogin.aspx" || sCaller == "Login" || sCaller == "Login.aspx") {
            sCaller = "Default.aspx";
        }
    }

    //we don't really care what is passed in for suppressMsgs any non-null value will evaluate to true;
    if (typeof (suppressMsgs) === 'undefined' || suppressMsgs === null || suppressMsgs.toString().toLowerCase() === 'false') {
        suppressMsgs = false;
    } else {
        suppressMsgs = true;
    }
    if (ssoaControl === "1") {
        return getNGLCachedToken(suppressMsgs, ucRequired, sCaller);
    }
    else {
        //we don't really care what is passed in for ucRequired any non-null value will evaluate to true;
        if (typeof (ucRequired) === 'undefined' || ucRequired === null || ucRequired.toString().toLowerCase() === 'false') {
            ucRequired = false;
        } else {
            ucRequired = true;
        }


        //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent NGL token logic
        var cToken = ADAL.getCachedToken(ADAL.config.clientId);
        //debugger;
        var blnReloadPage = false;

        if (cToken != null) {
            var user = ADAL.getCachedUser();
            if (user != null) {
                blnReloadPage = true;
                var divWelcome = document.getElementById('WelcomeMessage');
                var sName = user.profile.name;
                if (sName.trim() === '') {
                    sName = user.profile.given_name + ' ' + user.profile.family_name;
                }
                if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Welcome " + sName; }
                localStorage.SignedIn = "t";
                var signInMsg = document.getElementById('signInText');
                if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign Out"; }
                //hideloginNotification();
                var blnLoggedIn = postSSOResults(user, cToken);
                if (blnLoggedIn === false) {
                    localStorage.SignedIn = "f";
                    if (ucRequired === true) {
                        document.location = "../NGLLogin.aspx?caller=" + sCaller;
                        blnReloadPage = true;
                    } else {
                        var divWelcome = document.getElementById('WelcomeMessage');
                        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Authentication Required.  You are not logged in."; }

                        var signInMsg = document.getElementById('signInText');
                        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign In"; }
                        if (suppressMsgs === false) { showloginNotification("Not Authorized", "Please Sign In to Continue"); }

                        blnReloadPage = false;
                    }
                }
            } else {
                ADAL.acquireToken(ADAL.config.clientId, validateToken);
            }
        } else {
            blnReloadPage = false;
            localStorage.SignedIn = "f";
            if (ucRequired === true) {
                document.location = "../NGLLogin.aspx?caller=" + sCaller;
                blnReloadPage = true;
            } else {
                //signIn();
                var divWelcome = document.getElementById('WelcomeMessage');
                if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Authentication Required.  You are not logged in."; }

                var signInMsg = document.getElementById('signInText');
                if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign In"; }
                if (suppressMsgs === false) { showloginNotification("Not Authorized", "Please Sign In to Continue") }
            }
        }
        return blnReloadPage;
    }
}

// on success we return true and the calling function should redirect to Login.aspx to save the user sessions data
// modified by RHR for v-8.4.0.003 added optional parameter for sCaller
//  we use getCurentFileName as the default 
function getNGLCachedToken(suppressMsgs, ucRequired, sCaller) {
    //debugger;
    if (!ngl.stringHasValue(sCaller)) {
        sCaller = getCurentFileName();
        if (sCaller == "NGLLogin" || sCaller == "NGLLogin.aspx" || sCaller == "Login" || sCaller == "Login.aspx") {
            sCaller = "Default.aspx";
        }
    }
    var blnReturn = false;
    localStorage.SignedIn = "f";
    //we don't really care what is passed in for suppressMsgs any non-null value will evaluate to true;
    if (typeof (suppressMsgs) === 'undefined' || suppressMsgs === null || suppressMsgs.toString().toLowerCase() === 'false') {
        suppressMsgs = false;
    } else {
        suppressMsgs = true;
    }
    //we don't really care what is passed in for ucRequired any non-null value will evaluate to true;
    if (typeof (ucRequired) === 'undefined' || ucRequired === null || ucRequired.toString().toLowerCase() === 'false') {
        ucRequired = false;
    } else {
        ucRequired = true;
    }
    var sfilter = new NGLClass14();
    sfilter.NGLvar1452 = localStorage.NGLvar1452;
    sfilter.NGLvar1455 = localStorage.NGLvar1455;
    sfilter.NGLvar1454 = localStorage.NGLvar1454;
    var ssoaExp = localStorage.SSOAExpiresMilli;
    var current = Date.now();
    //debugger;
    if (typeof (ssoaExp) !== 'undefined' && ssoaExp !== null && current < ssoaExp && localStorage.NGLvar1452.toString() !== "0") {
        //debugger;
        $.ajax({
            async: false,
            type: "GET",
            url: "api/SSOA/NGLLegacyValidateToken",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { filter: JSON.stringify(sfilter) },
            success: function (data) {
                try {
                    var blnSuccess = false;
                    var blnErrorShown = false;
                    var strValidationMsg = "";
                    //debugger;
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                            blnErrorShown = true;
                            if (ucRequired === true) {
                                // user must log in 
                                document.location = "../NGLLogin.aspx?caller=" + sCaller;
                            } else {
                                if (suppressMsgs === false) { ngl.showErrMsg("User Login Validation Failure", data.Errors, null); }
                            }
                        }
                        else {
                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && data.Data[0] !== null) {
                                    blnSuccess = true;
                                    dataitem = data.Data[0];
                                    if  (data.Data[0].toString() === "true"){
                                        blnReturn = true;
                                        localStorage.SignedIn = "t";
                                        var divWelcome = document.getElementById('WelcomeMessage');
                                        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                            divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
                                        }

                                        var signInMsg = document.getElementById('signInText');
                                        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {
                                            signInMsg.innerHTML = "Sign Out";
                                        }
                                        return true;
                                    } else {
                                        // user must login 
                                        blnErrorShown = true;
                                        if (ucRequired === true) {
                                            document.location = "../NGLLogin.aspx?caller=" + sCaller;
                                        } else {
                                            if (suppressMsgs === false) { ngl.showErrMsg("User Login Validation Failure", data.Errors, null); }
                                        }
                                        return false;
                                    }
                                    
                                }
                            }
                        }
                    }
                    if (blnSuccess === false && blnErrorShown === false) {
                        // User must Login
                        if (ucRequired === true) {
                            document.location = "../NGLLogin.aspx?caller=" + sCaller;
                        } else {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Invalid Please Try Again"; }
                            if (suppressMsgs === false) { ngl.showErrMsg("User Login Validation Failure", strValidationMsg, null); }
                        }
                        return false;
                    }
                } catch (err) {
                    ngl.showErrMsg(err.name, err.description, null);
                }
            },
            error: function (xhr, textStatus, error) {
                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Authentication Failure");
                ngl.showErrMsg("User Login Validation", sMsg, null);
                return false;
            }
        });
       
    } else { 
       

        $.ajax({
            async: false,
            type: "GET",
            url: "api/SSOA/GetNGLSSOAToken",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { filter: JSON.stringify(sfilter) },
            success: function (data) {
                try {
                    var blnSuccess = false;
                    var blnErrorShown = false;
                    var strValidationMsg = "";
                    //debugger;
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                            blnErrorShown = true;
                            // failed user must log in
                            if (ucRequired === true) {
                                document.location = "../NGLLogin.aspx?caller=" + sCaller;
                            } else {
                                if (suppressMsgs === false) { ngl.showErrMsg("User Login Validation Failure", data.Errors, null); }
                            }
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
                                    if (blnSuccess === true && 'SSOAExpiresMilli' in dataitem) { localStorage.SSOAExpiresMilli = dataitem.SSOAExpiresMilli; }
                                    //for now we do not do anything with the read method
                                    //this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                                    if (blnSuccess === true) {
                                        blnReturn = true;
                                        localStorage.SignedIn = "t";
                                        var divWelcome = document.getElementById('WelcomeMessage');
                                        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                                            divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455; 
                                        }

                                        var signInMsg = document.getElementById('signInText');
                                        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {
                                            signInMsg.innerHTML = "Sign Out";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (blnSuccess === false && blnErrorShown === false) {
                        if (ucRequired === true) {
                            // user must login again
                            document.location = "../NGLLogin.aspx?caller=" + sCaller;
                        } else {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Account information not found"; }
                            if (suppressMsgs === false) { ngl.showErrMsg("User Login Validation Failure", strValidationMsg, null); }
                        }
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
    return blnReturn;
}


//check if an existing token is available and refresh the JWT user info
//Call the SSOA REST Service to save the Token info on the server
function updateValidUserMsgs() {
    //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent NGL token logic
    var cToken = ADAL.getCachedToken(ADAL.config.clientId);   
    var divWelcome = document.getElementById('WelcomeMessage');
    var signInMsg = document.getElementById('signInText');
    var blnReloadPage = false;
    if (cToken != null) {
        var user = ADAL.getCachedUser();
        if (user != null) {
            blnReloadPage = true;
            var sName = user.profile.name;
            if (sName.trim() === '') {
                sName = user.profile.given_name + ' ' + user.profile.family_name;
            }
            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
                divWelcome.innerHTML = "Welcome " + sName;
            }
            localStorage.SignedIn = "t";
            
            if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {
                signInMsg.innerHTML = "Sign Out";
            }
            
            hideloginNotification();
            
        } else {
            ADAL.acquireToken(ADAL.config.clientId, validateToken);
        }
    } else {
        blnReloadPage = false;
        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
            divWelcome.innerHTML = "Authentication Required.  You are not logged in.";
        }        
        localStorage.SignedIn = "f";
        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) {
            signInMsg.innerHTML = "Sign In";
        }        
        showloginNotification("Not Authorized", "Please Sign In to Continue")
    }
    return blnReloadPage;
}


//check if an existing token is available and refresh the JWT user info
//Call the SSOA REST Service to save the Token info on the server
function validateLogin(caller) {
    //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent NGL token logic
    var cToken = ADAL.getCachedToken(ADAL.config.clientId);

    var blnReloadPage = false;

    if (cToken != null) {
        var user = ADAL.getCachedUser();
        if (user != null) {
            blnReloadPage = true;
            var divWelcome = document.getElementById('WelcomeMessage');
            var sName = user.profile.name;
            if (sName.trim() === '') {
                sName = user.profile.given_name + ' ' + user.profile.family_name;
            }
            if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Welcome " + sName; }
            localStorage.SignedIn = "t";           
            return postLoginResults(user, cToken);
        } else {
            ADAL.acquireToken(ADAL.config.clientId, validateToken);
            return false;
        }
    } else {
        var divWelcome = document.getElementById('WelcomeMessage');
        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Authentication Required.  You are not logged in."; }
        localStorage.SignedIn = "f";        
        showloginNotification("Not Authorized", "Please Sign In to Continue");
        return false;
    }
    return false;
}

//get the JWT user data, show welcome message and call SSOA REST Service to save the token information on the server
function showWelcomeMessage() {
    validateUser();

    //alert("showWelcomeMessage");
    //var user = ADAL.getCachedUser();
    //var divWelcome = document.getElementById('WelcomeMessage');
    //var sName = user.profile.name;
    //if (sName.trim() === '') {
    //    sName = user.profile.given_name + ' ' + user.profile.family_name;
    //}
    //divWelcome.innerHTML = "Welcome " + sName;
    //alert("Welcome " + user.profile.name);
}

//if error show error in WelcomeMessage div tag else call showWelcomeMessage function
//May need additional logic to call the SSOA controller method to clear out the users stored token on error
function validateToken(err, token) {
    //alert("validateToken");
    if (err) {
        var divWelcome = document.getElementById('WelcomeMessage');
        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Authentication Required.  Your are not logged in."; }
        localStorage.SignedIn = "f";
        var signInMsg = document.getElementById('signInText');
        if (typeof (signInMsg) !== 'undefined' && ngl.isObject(signInMsg)) { signInMsg.innerHTML = "Sign In"; }
        showloginNotification("Not Authorized", "Please Sign In to Continue")
        
    }
    else {
        //alert("validateToken --> showWelcomeMessage");
        showWelcomeMessage();
    }
    
}

function validateCallerToken(err, token) {
    //alert("validateToken");
    if (err) {
        var divWelcome = document.getElementById('WelcomeMessage');
        if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Authentication Required.  Your are not logged in."; }
        localStorage.SignedIn = "f";

    }
    else {
       var uc = localStorage.NGLvar1452;
       document.location = "../" + caller + "?uc=" + uc;
    }

}



//show error message in WelcomeMessage div tag
function showError(error) {
    //alert("showError");
    console.error(error);
    var divWelcome = document.getElementById('WelcomeMessage');
    if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Error: " + error; }
}

//test if the token has expired
function hasTokenExpired(exp) {
    //we divide by 1000 to convert milliseconds to seconds
    //Date.now() us based on UTC timestamp
    if (exp < (Date.now() / 1000)) {
        return true;
    } else {
        return false;
    }
}

//test if the token will expire in intSeconds
function willTokenExpire(exp, intSeconds) {
    //we divide by 1000 to convert milliseconds to seconds
    //Date.now() us based on UTC timestamp
    if (exp < (Date.now() / 1000) + intSeconds) {
        return true;
    } else {
        return false;
    }
}

////function getCachedNGLToken() {
////    //alert("validateUser");
////    //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent
////    //NGL token logic
////    return localStorage.nglT;
////}

////function NGLSignIn() {
////    //alert("validateUser");
////    //Read HTML5 local storage for NGL token and if token exists we bypass the ADAL logic and implemnent
////    //NGL token logic
////    return localStorage.nglT;
////}

function showloginNotification(sTitle, sMsg) {
    if (typeof (notification) != 'undefined' && notification != null) {
        notification.show({
            title: sTitle,
            message: sMsg
        }, "info");
    }

}

function hideloginNotification() {
    if (typeof (notification) != 'undefined' && notification != null) {
        notification.hide();
    }
}


//AuthenticationContext.prototype.getCachedToken =
//    function (a) {
//        if (!this._hasResource(a)) return null;
//        var b = this._getItem(this.CONSTANTS.STORAGE.ACCESS_TOKEN_KEY + a), c = this._getItem(this.CONSTANTS.STORAGE.EXPIRATION_KEY + a), d = this.config.expireOffsetSeconds || 300; return c && c > this._now() + d ? b : (this._saveItem(this.CONSTANTS.STORAGE.ACCESS_TOKEN_KEY + a, ""), this._saveItem(this.CONSTANTS.STORAGE.EXPIRATION_KEY + a, 0), null)
//    }



