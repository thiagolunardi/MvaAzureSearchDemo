using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace AzureSearchWebApiDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
#if DEBUG
            config.Formatters.JsonFormatter.Indent = true;
#endif

            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultApi",routeTemplate: "{controller}/{action}/{searchText}");
        }
    }
}
