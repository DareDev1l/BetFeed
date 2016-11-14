using Autofac;
using Autofac.Integration.WebApi;
using BetFeed.Infrastructure;
using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace BetFeed.App_Start
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            // Get HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Register DbContext
            builder.RegisterType<BetFeedContext>().AsSelf();

            // Register generic repository
            builder.RegisterAssemblyTypes(typeof(IRepository<Sport>).Assembly)
                .Where(t => t.Name.Equals("IRepository"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            //Register custom repositories
            builder.RegisterType<EfRepository<Sport>>().As<IRepository<Sport>>();

            // Register Services
            builder.RegisterAssemblyTypes(typeof(VitalbetService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerRequest();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}