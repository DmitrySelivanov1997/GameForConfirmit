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
using Unity;
using Unity.Lifetime;
using WebGameService.Models;
using System.Web.Http.Dependencies;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;

namespace WebGameService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<GameSessionStatistic>("GameSessionStatistics");
            IEdmModel model = modelBuilder.GetEdmModel();
            config.Routes.MapODataRoute(routeName: "OData", routePrefix: "odata", model: model);

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());

            config.Formatters.Add(new BsonMediaTypeFormatter());

            var container = new UnityContainer();
            container.RegisterType<ISessionRepository, SessionRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
