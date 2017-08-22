using AuroraOs.Common.Core.Manager;
using AuroraOs.Web.Api.Core.Filters;
using Microsoft.Owin.Cors;
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

namespace AuroraOs.Web.Api.Core
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
            var manager = new AuroraManager();

            var config = new HttpConfiguration()
            {
                DependencyResolver = new UnityDependencyResolver(manager.Container)
            };

            AddFilters(config);
            UseJsonOutput(config);
            UseSwagger(config);

            appBuilder.UseCors(CorsOptions.AllowAll);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/"
            );

            config.MapHttpAttributeRoutes();


            appBuilder.UseWebApi(config);
        }
    }
}
