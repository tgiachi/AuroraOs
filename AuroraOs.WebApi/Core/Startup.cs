﻿using AuroraOs.WebApi.Core.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Unity.WebApi;

namespace AuroraOs.WebApi.Core
{
    /// <summary>
    /// Startup class for OWIN
    /// </summary>
    public class Startup
    {

        private void UseJsonOutput(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        private void UseSwagger(HttpConfiguration config)
        {
            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "AuroraOs API"))
                .EnableSwaggerUi();
        }

        private void AddFilters(HttpConfiguration config)
        {
            config.Filters.Add(new LoggingFilterAttribute());
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration()
            {
                DependencyResolver = new UnityDependencyResolver(new UnityConfig().Container.Value)
            };

            AddFilters(config);
            UseJsonOutput(config);
            UseSwagger(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
