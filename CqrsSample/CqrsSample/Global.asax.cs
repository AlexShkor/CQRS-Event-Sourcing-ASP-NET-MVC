using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CqrsSample.Domain;
using CqrsSample.EventHandlers;
using Paralect.Domain;
using Paralect.Domain.EventBus;
using Paralect.ServiceBus;
using Paralect.ServiceBus.Dispatching;
using Paralect.ServiceLocator.StructureMap;
using Paralect.Transitions;
using StructureMap;

namespace CqrsSample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var container = new Container();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));

            //
            // Service bus
            //
            var bus = ServiceBus.Run(c => c
                .SetServiceLocator(new StructureMapServiceLocator(container))
                .MemorySynchronousTransport()
                .SetName("Main  Service Bus")
                .SetInputQueue("sync.server")
                .AddEndpoint(type => type.FullName.EndsWith("Event"), "sync.server")
                .AddEndpoint(type => type.FullName.EndsWith("Command"), "sync.server")
                .AddEndpoint(type => type.FullName.EndsWith("Message"), "sync.server")
                .Dispatcher(d => d
                    .AddHandlers(typeof(UserAR).Assembly) 
                     .AddHandlers(typeof(UserEntityEventHandler).Assembly)
                 )
            );

            container.Configure(config => config.For<IServiceBus>().Singleton().Use(bus));

            // 
            // Domain and Event store configuration
            //
            var dataTypeRegistry = new AssemblyQualifiedDataTypeRegistry();


            var transitionsRepository = new InMemoryTransitionRepository();

            var transitionsStorage = new TransitionStorage(transitionsRepository);

            container.Configure(config =>
            {
                config.For<ITransitionRepository>().Singleton().Use(transitionsRepository);
                config.For<ITransitionStorage>().Singleton().Use(transitionsStorage);
                config.For<IDataTypeRegistry>().Singleton().Use(dataTypeRegistry);
                config.For<IEventBus>().Use<ParalectServiceBusEventBus>();
                config.For<IRepository>().Use<Repository>();
                config.For<ICommandService>().Use<CommandService>();
            });

        }
    }
}