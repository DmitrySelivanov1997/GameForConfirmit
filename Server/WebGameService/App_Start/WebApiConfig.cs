using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Windows;
using WebGameService.Models;

namespace WebGameService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

           // config.Filters.Add(new CustomHeaderFilter());
            // Web API configuration and services 

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());
            config.Formatters.Add(new BsonMediaTypeFormatter());
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );
        }
    }
}
