using Autofac;
using Autofac.Integration.WebApi;
using BetFeed.Infrastructure;
using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System;
using BetFeed.ViewModels;
using AutoMapper;

namespace BetFeed.App_Start
{
    public static class Bootstrapper
    {
        // This class breaks single responsibility, extract those 2 methods in different classes :
        // - AutoFacCondif
        // - AutoMapperConfig
        // Then call the static methods in global.asax
        public static void Run()
        {
            SetAutofacContainer();
            RegisterAutoMapper();
        }

        private static void RegisterAutoMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Sport, SportViewModel>();
                cfg.CreateMap<Event, EventViewModel>();
                cfg.CreateMap<Match, MatchViewModel>();
            });

            Mapper.AssertConfigurationIsValid();
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
            builder.RegisterType<SportRepository>().As<IRepository<Sport>>();
            builder.RegisterType<EfRepository<Event>>().As<IRepository<Event>>();
            builder.RegisterType<EfRepository<Bet>>().As<IRepository<Bet>>();
            builder.RegisterType<EfRepository<Match>>().As<IRepository<Match>>();
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