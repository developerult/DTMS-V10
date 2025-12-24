<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateShop.aspx.cs" Inherits="DynamicsTMS365.RateShop" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Rate Shopping Redirect From Desktop</title>          
         <%=cssReference%>             
        <style>
            html,
body{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
       </style> 

    </head>
    <body>       
        <%=jssplitter2Scripts%>
        <%=sWaitMessage%>      
    <div>
    <% Response.Write(PageErrorsOrWarnings); %>
    </div>
    <script>
        
        <% Response.Write(ADALPropertiesjs); %>
        if('<% Response.Write(blnUseSSR.ToString().ToLower()); %>' == 'true' ) {
            localStorage.NGLvar1455 = '<%  Response.Write(this.UserName.Replace(@"\", @"\\")); %>';
            localStorage.NGLvar1452 = '<% Response.Write(this.SSOR.UserSecurityControl); %>'; 
            localStorage.NGLvar1451 = '<% Response.Write(this.SSOR.USATUserID); %>'; 
            localStorage.NGLvar1454 = '<% Response.Write(this.SSOR.USATToken); %>'; 
            localStorage.NGLvar1472 = '<% Response.Write(this.SSOR.SSOAControl); %>'; 
            localStorage.NGLvar1458 = '<% Response.Write(this.SSOR.SSOAUserEmail); %>'; 
            localStorage.NGLvar1474 = '';
            localStorage.NGLvar1457 = '<% Response.Write(this.SSOR.UserFriendlyName); %>'; 
		
            //not used in RateShop Redirect Page
            //var divWelcome = document.getElementById('WelcomeMessage');
            //if (typeof (divWelcome) !== 'undefined' && ngl.isObject(divWelcome)) {
            //    divWelcome.innerHTML = "Welcome " + localStorage.NGLvar1455;
            //}           
            localStorage.SignedIn = "t";

        }
        var uc = localStorage.NGLvar1452;
        document.location = "RateShopping?uc=" + uc;
    </script>
</body>
</html>
