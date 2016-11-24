using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BetFeed
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "BetFeedApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "UpdateRoutes",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
