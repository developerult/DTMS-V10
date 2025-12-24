<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOut.aspx.cs" Inherits="DynamicsTMS365.LogOut" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS LogOut</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

</style>
    </head>
    <body>       
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div class="pane-content"> 
                        <span style="float:left; display:inline-block;">                       
                            <span style="margin:6px; vertical-align: middle;">
                                <a href="Default.aspx"><img border="0" alt="Home" src="Content/NGL/Home32.png" width="32" height="32"></a>
                            </span>
                            <span style="margin:6px; vertical-align: middle;" >
                                <a href="http://www.nextgeneration.com"><img border="0" alt="NGL" src="../Content/NGL/nextracklogo.GIF" ></a>
                            </span>
                        </span>
                        <span style="float:right; display:inline-block;">
                            <span style="margin:6px; vertical-align: middle;">
                                    <a href="Settings.aspx"><img border="0" alt="Settings" src="Content/NGL/Settings32.png" width="32" height="32"></a>
                                </span>
                                <span style="margin:6px; vertical-align: middle;" >
                                    <a href="http://nglwcfdev705.nextgeneration.com/usermanual"><img border="0" alt="Help" src="../Content/NGL/Help32.png" ></a>
                                </span>
                        </span>
                    </div>
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>Menu</span></div>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">
                            <div class="pane-content">
                                <div><span>LogOut</span></div>                                 
                            </div>
                        </div>
                        <div id="right-pane">
                            <div class="pane-content">
                                <div><span>Right</span></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" style="height: 100%; width: 100%; ">
                    <div class="pane-content">
                        <div><span>Footer</span></div>
                    </div>
                </div>
            </div>

            
    <script>
        //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        //var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        //loadAuthContext();
        //NOTE:   validateUser(); must be called in the docuemnt.ready function
        //End ADAL properties

        $(document).ready(function () {
            document.location = '<% Response.Write(WebBaseURI); %>' +"NGLLogin.aspx";
            return;          
        });


    </script>
    <style>

                #vertical {
                    height: auto;
                    margin: 0 auto;
                }
                #menu-pane { background-color: white;  }
                #middle-pane { background-color: white; }
                #bottom-pane { background-color: white;}
                #left-pane, #center-pane, #right-pane  { background-color: white;  }

                .pane-content {
                    padding: 0 10px;
                }


            </style>
    </div>


    </body>

</html>

