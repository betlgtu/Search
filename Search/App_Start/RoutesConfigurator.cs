using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Search.App_Start
{
    public class RoutesConfigurator : IConfigurator
    {
        public void Configure()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}