using Autofac;
using Autofac.Integration.WebApi;
using BetFeed.Infrastructure;
using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services;
using System.Reflection;
using System.Web.Http;

namespace BetFeed.App_Start
{
    public static class AutoFacConfiguration
    {
        public static void Configure()
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
            builder.RegisterType<SportRepository>().As<IRepository<Sport>>();
            builder.RegisterType<EfRepository<Event>>().As<IRepository<Event>>();
            builder.RegisterType<EfRepository<Bet>>().As<IRepository<Bet>>();
            builder.RegisterType<MatchRepository>().As<IRepository<Match>>();
            builder.RegisterType<EfRepository<Odd>>().As<IRepository<Odd>>();

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