using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            config.Routes.MapHttpRoute(
name: "API Default",
routeTemplate: "api/{controller}/{action}/{id}",
defaults: new { id = RouteParameter.Optional }
            );
            //            config.Routes.MapHttpRoute(
            //  name: "ApiByAction",
            //  routeTemplate: "api/values/UserAuthenticate/{username}/{password}/{email}/{address}",
            //  defaults: new { controller = "values", action = "UserAuthenticate" }
            //);
           
            
            //config.Routes.MapHttpRoute(
            //  name: "ApiByAction",
            // //routeTemplate: "api/values/UserAuthenticate/{firstname}/{lastname}/{company}/{contactno}/{email}/{address}/{country}/{state}/{city}/{postalcode}/{subscriptiontype}/{orgname}/{serverurl}/{username}/{password}",
            // routeTemplate: "api/values/UserAuthenticate/{firstname}/{lastname}/{company}/{contactno}/{email}/{address}/{country}/{state}/{city}/{postalcode}/{subscriptiontype}/{orgname}/{serverurl}/{username}/{password}",
            //  defaults: new { controller = "values", action = "UserAuthenticate" }
            //);
        }
    }
}
