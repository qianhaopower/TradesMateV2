using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using EF.Data;
using System.Net.Http.Formatting;

namespace DataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
           
           
            // Web API routes
            config.MapHttpAttributeRoutes();
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            ////odata route
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<Property>("Properties");
            builder.EntitySet<WorkItem>("WorkItems");


            builder.EntitySet<WorkItemTemplate>("WorkItemTemplates");
            builder.EntitySet<Section>("Sections");
            builder.EntitySet<Company>("Companies");
            builder.EnableLowerCamelCase();
            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());





            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );




        }
    }
}
