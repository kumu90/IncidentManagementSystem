using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IncidentManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );


            /// custom for ticket
            //routes.MapRoute(
            //name: "TicketDetail",
            //url: "Home/TicketSearch/{TicketId}",
            //defaults: new { controller = "Home", action = "TicketSearch", TicketId = UrlParameter.Optional }
            //);


            //// Custom route 
            //routes.MapRoute(
            //    name: "InstitutionService",
            //    url: "Institution/GetService/{InstId}",
            //    defaults: new { controller = "Institution", action = "GetService" }
            //);
        }
    }
}
