<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DynamicsTMS365.Login" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Login Redirect</title>          
         <%=cssReference%>             
        <style>
            html,
            body {height:100%; margin:0; padding:0;}
            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
       </style> 
    </head>
    <body>       
        <%=jssplitter2Scripts%>
        <%=sWaitMessage%>      
    <div>
    <% Response.Write(PageErrorsOrWarnings); %>
    </div>
    <script> 
        //debugger;
        var sCaller = '<%=sCaller%>'; // read the caller from the code behind property
        
        var control = <%=UserControl%>; // read the user control from the code behind property this will normally be zero if we get here
        <% Response.Write(ADALPropertiesjs); %>
        //  sbScript.Append("var opostLogoutRedirectUri = '" + WebBaseURI + "'; ");
        //        sbScript.Append(sCRLF);
       // sbScript.Append("var oredirectUri = '" + WebBaseURI + "' + getCurentFileName(); ");
        if('<% Response.Write(blnUseSSR.ToString().ToLower()); %>' == 'true' ) {
            localStorage.NGLvar1455 = '<%  Response.Write(this.UserName.Replace(@"\", @"\\")); %>';
            localStorage.NGLvar1452 = '<% Response.Write(this.SSOR.UserSecurityControl); %>'; 
            localStorage.NGLvar1451 = '<% Response.Write(this.SSOR.USATUserID); %>'; 
            localStorage.NGLvar1454 = '<% Response.Write(this.SSOR.USATToken); %>'; 
            localStorage.NGLvar1472 = '<% Response.Write(this.SSOR.SSOAControl); %>'; 
            localStorage.NGLvar1458 = '<% Response.Write(this.SSOR.SSOAUserEmail); %>'; 
            localStorage.NGLvar1474 = '';
            localStorage.NGLvar1457 = '<% Response.Write(this.SSOR.UserFriendlyName); %>'; 
		
            //not used in Login Redirect Page
            //var divWelcome = document.getElementById('WelcomeMessage');
            //if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) { divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455; }           
            localStorage.SignedIn = "t";
            //if we get here just go to the home page.  this should not happen
            var uc = localStorage.NGLvar1452;
            //debugger;
            document.location =  "../Default?uc=" + uc;
        } else if (control == 0 && ngl.stringHasValue(sCaller)) {
            // Rule  3. if user control is 0 but caller is not let Javascript read local storage and authenticate the user with local data
            ///         SSOA js will redirect back to Login.aspx page after authentication with valid data 
            ///         or it will redirect to NGLLogin page
            // set value of oredirectUri to use caller
            oredirectUri = opostLogoutRedirectUri + '/' + sCaller;
            //UserValidated365 will attempt to read the local storeage for the user 
            //information,  then validate the users token
            // if no user control in local storeage the system
            // will return to the Log in screen
            // note,  an optional serverUserControl lets the system
            // know that the code behind session has been populated
            ngl.UserValidated365(true,0,sCaller);  
        }
       
    </script>
</body>
</html>
