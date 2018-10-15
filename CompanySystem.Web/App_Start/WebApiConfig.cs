﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using CompanySystem.Web.Filters;

namespace CompanySystem.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableSystemDiagnosticsTracing();

            //Enforce filter over the entire Web API
            //config.Filters.Add(new ForceHttpsAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Companies",
                routeTemplate: "api/CompanySystemV1/{action}/{id}",
                defaults: new { controller = "Companies", id = RouteParameter.Optional }
                //defaults: new { id = RouteParameter.Optional }
            );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
