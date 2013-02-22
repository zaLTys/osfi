using System.Configuration;
using System.Security.Cryptography;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentValidation;
using FluentValidation.Mvc;
using NHibernate;
using Oracle.DataAccess.Client;
using PetaPoco;
using StatistikosFormos.FormuValidavimas;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Security;
using StatistinesAtaskaitos.Services;
using StatistinesAtaskaitos.Validators;
using Vic.ZubStatistika.DataAccess;
using Vic.ZubStatistika.DataAccess.Mappings;
using Vic.ZubStatistika.Entities;

[assembly: WebActivator.PreApplicationStartMethod(typeof(StatistinesAtaskaitos.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(StatistinesAtaskaitos.App_Start.NinjectWebCommon), "Stop")]

namespace StatistinesAtaskaitos.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Settings.AllowNullInjection = true;
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var connString = ConfigurationManager.ConnectionStrings["OSFI"].ConnectionString;
            var schemaName = ConfigurationManager.AppSettings["SchemaName"];

            kernel.Bind<Database>().ToConstructor(x => new Database(connString, new OracleClientFactory()));
            kernel.Bind<IStatistiniuAtaskaituService>().To<StatistiniuAtaskaituService>();
            kernel.Bind<IAuthenticationProvider>().To<FormsAuthenticationProvider>();
            kernel.Bind<HashAlgorithm>().To<SHA256Managed>();
            kernel.Bind<IImoniuService>().To<ImoniuService>();
            kernel.Bind<ISessionFactory>().ToMethod(ctx =>
                                                        {
                                                            var config = Fluently.Configure()
                                                                .Database(OracleDataClientConfiguration.Oracle10
                                                                              .ConnectionString(connString).
                                                                              DefaultSchema(schemaName))
                                                                .Mappings(m => m.AutoMappings.Add(
                                                                    AutoMap.AssemblyOf<Augalininkyste>()
                                                                        .Conventions.Add(
                                                                            new NHibernateSequenceConvention(),
                                                                            new CascadeAllConvention(),
                                                                            Table.Is(x => x.TableName.ToUpper()))
                                                                            .UseOverridesFromAssemblyOf<UploadMapOverride>));

                                                                return config.BuildSessionFactory();
                                                        }).InSingletonScope();
            kernel.Bind<UserInformation>()
                .ToMethod(ctx =>
                              {
                                  var user = HttpContext.Current.User as OsfiPrincipal;
                                  if (user == null) return null;
                                  return user.User;
                              })
                .WhenTargetHas<LoggedInAttribute>();

            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new NinjectValidatorFactory(kernel));

            kernel.Bind<IValidator<UserCreateModel>>().To<UserCreateValidator>();
            kernel.Bind<IValidator<SlaptazodzioKeitimasModel>>().To<PasswordChangeValidator>();
            kernel.Bind<IUploadValidator>().To<UploadValidator>();
        }
    }
}
