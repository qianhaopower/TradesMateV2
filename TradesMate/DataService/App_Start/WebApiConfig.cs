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
using DataService.Models;

namespace DataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            // Web API routes
     //       config.MapHttpAttributeRoutes();
     //       var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
     //       jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

     //       config.Routes.MapHttpRoute(
     //    name: "DefaultApi",
     //    routeTemplate: "api/{controller}/{id}",
     //    defaults: new { id = RouteParameter.Optional }
     //);

     //       config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.EnableUnqualifiedNameCall(unqualifiedNameCall: true);
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ////odata route
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Client>("Clients");
            builder.EntitySet<Address>("Addresses");


            builder.EntitySet<Property>("Properties");

            //builder.Namespace = "PropertyExtension";
            //builder.EntityType<Property>()
            //    .Function("GetCompanies")
            //    .ReturnsCollection<CompanyModel>();


            //FunctionConfiguration function = builder.EntityType<Property>().Function("GetCompanies").Returns<bool>();
            //function.Parameter<int>("ptX");
            //function.Parameter<int>("ptY");

            // bound to entity and return the related entity
            FunctionConfiguration function = builder.EntityType<Property>().Function("GetCompanies").ReturnsCollection<CompanyModel>(); ;
            // function.Parameter<double>("area");
            // function.IsComposable = true;


            ////universal function
            // GET / DataService / odata / GetPropertyCompanies(propertyId = 1) HTTP / 1.1
            //builder.Function("GetPropertyCompanies")
            //    .ReturnsCollection<CompanyModel>()
            //    .Parameter<int>("propertyId");

            //  GET / DataService / odata / GetMemberAllocation(memberId = 1) HTTP / 1.1
            builder.Function("GetMemberAllocation")
                .ReturnsCollection<AllocationModel>()
                .Parameter<int>("memberId");

            var functionUpdateMemberAllocation = builder.Function("UpdateMemberAllocation");
            functionUpdateMemberAllocation.Parameter<int>("propertyId");
            functionUpdateMemberAllocation.Parameter<int>("memberId");
            functionUpdateMemberAllocation.Parameter<bool>("allocated");
            functionUpdateMemberAllocation.Returns<AllocationModel>();
            



            builder.EntityType<Property>().Function("SomeFunction").Returns<string>();


            builder.EntitySet<WorkItem>("WorkItems");
            builder.EntitySet<WorkItemTemplate>("WorkItemTemplates");
            builder.EntitySet<Section>("Sections");
            builder.EntitySet<Company>("Companies");
            builder.EnableLowerCamelCase();



            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());





        }
    }
}
