﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using Ninject;
using StatistinesAtaskaitos.Security;

namespace StatistinesAtaskaitos
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelMetadataProviders.Current = new DecimalMetadataProvider();

            PostAuthenticateRequest += Application_PostAuthenticateRequest;
        }

        void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var provider = DependencyResolver.Current.GetService<IAuthenticationProvider>();
            provider.TryAuthenticate();
        }
    }
}