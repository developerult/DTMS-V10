using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace DynamicsTMS365
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //RouteTable.Routes.MapHttpRoute(
            //name: "UserTokenApi",
            //routeTemplate: "api/{controller}/{action}/{id}/{usercontrol}/{token}",
            //defaults: new { controller = "", action = "", id = 0, usercontrol = 0, token = "" });

            RouteTable.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = System.Web.Http.RouteParameter.Optional },
               constraints: new { id = @"^-?\d+$" } // id must be digits
               //constraints: new { id = @"^\d+$" } // id must be digits
               ///^-?\d{2}(\.\d+)?$/
               );

           RouteTable.Routes.MapHttpRoute(
            name: "ActionApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = System.Web.Http.RouteParameter.Optional },
             constraints: new { id = @"^-?\d+$" } // id must be digits
             //constraints: new { id = @"^\d+$" } // id must be digits

               );

            RouteTable.Routes.MapHttpRoute(
          name: "NoFilterApi",
          routeTemplate: "api/{controller}/{action}/{filter}",
          defaults: new { filter = System.Web.Http.RouteParameter.Optional });


            //RouteTable.Routes.MapHttpRoute(
            //   name: "DefaultPKByUserApi",
            //   routeTemplate: "api/{controller}/{id}/{usercontrol}",
            //   defaults: new { id = System.Web.Http.RouteParameter.Optional },
            //   constraints: new { id = @"^\d+$", usercontrol = @"^\d+$" } // id & usercontrol must be digits

            //   );


            //RouteTable.Routes.MapHttpRoute(
            //  name: "sFilterApi",
            //  routeTemplate: "api/{controller}/{filter}",
            //  defaults: new { filter = System.Web.Http.RouteParameter.Optional });

            //RouteTable.Routes.MapHttpRoute(
            //name: "FilterApi",
            //routeTemplate: "api/{controller}/{action}"
            //);

            //RouteTable.Routes.MapHttpRoute(
            //name: "TokenApi",
            //routeTemplate: "api/{controller}/{action}/{id}/{token}",
            //defaults: new { id = System.Web.Http.RouteParameter.Optional },
            //   constraints: new { id = @"^\d+$" } // id must be digits

            //   );

            //RouteTable.Routes.MapHttpRoute(
            //name: "PKByUserApi",
            //routeTemplate: "api/{controller}/{action}/{id}/{usercontrol}",
            //defaults: new { id = System.Web.Http.RouteParameter.Optional, usercontrol = System.Web.Http.RouteParameter.Optional },
            //   constraints: new { id = @"^\d+$", usercontrol = @"^\d+$" } // id & usercontrol must be digits

            //   );


            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}