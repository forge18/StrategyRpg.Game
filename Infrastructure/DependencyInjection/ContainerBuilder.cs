using System;
using Data;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;
using Infrastructure.Ecs.Systems;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Logging;
using Infrastructure.MediatorNS;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.EventManagement;
using Infrastructure.MediatorNS.QueryManagement;
using Infrastructure.MediatorNS.QueryManagement.Queries;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;
using Serilog;

namespace Infrastructure.DependencyInjection
{
    public class ContainerBuilder
    {
        public ServiceProvider Build()
        {
            var container = new ServiceCollection();

            container = AddServicesToCollection(container);
            container = AddFactoriesToCollection(container);
            container = AddCommandsToCollection(container);
            container = AddEventsToCollection(container);
            container = AddQueriesToCollection(container);
            container = AddMediatorToCollection(container);
            container = AddLoggingToCollection(container);

            return container.BuildServiceProvider();
        }

        public ServiceCollection AddServicesToCollection(ServiceCollection container)
        {
            container.AddTransient<IEcsDataLoader, EcsDataLoader>(
                provider => new EcsDataLoader(
                    provider.GetService<IEcsEntityService>(),
                    provider.GetService<IEcsQueryService>()
                )
            );
            container.AddSingleton<IEcsEntityService, EcsEntityService>(
                provider => new EcsEntityService(
                    provider.GetService<IEcsWorldService>(),
                    provider.GetService<IEcsQueryService>(),
                    provider.GetService<IEcsDataLoader>()
                )
            );
            container.AddSingleton<IEcsQueryService, EcsQueryService>(
                provider => new EcsQueryService(
                    provider.GetService<IEcsWorldService>()
                )
            );
            container.AddSingleton<IEcsSystemService, EcsSystemService>();
            container.AddSingleton<IEcsWorldService, EcsWorldService>();
            container.AddSingleton<INodeLocatorService, NodeLocatorService>();
            container.AddSingleton<INodeService, NodeService>(
                provider => new NodeService(
                    provider.GetService<INodeLocatorService>()
                )
            );
            container.AddSingleton<IServiceProvider, ServiceProvider>();

            return container;
        }

        public ServiceCollection AddFactoriesToCollection(ServiceCollection container)
        {
            container.AddTransient<ICommandFactory, CommandFactory>();
            container.AddTransient<IEventFactory, EventFactory>();
            container.AddTransient<IQueryFactory, QueryFactory>();

            return container;
        }

        public ServiceCollection AddCommandsToCollection(ServiceCollection container)
        {


            return container;
        }

        public ServiceCollection AddEventsToCollection(ServiceCollection container)
        {
            return container;
        }

        public ServiceCollection AddQueriesToCollection(ServiceCollection container)
        {
            container.AddTransient<IQueryHandler<GetEntitiesToRenderQuery>, GetEntitiesToRenderHandler>(
                provider => new GetEntitiesToRenderHandler(
                    provider.GetService<IEcsWorldService>()
                )
            );

            return container;
        }

        public ServiceCollection AddMediatorToCollection(ServiceCollection container)
        {
            container.AddSingleton<ICommandMediator, CommandMediator>(
                provider => new CommandMediator(
                    provider.GetService<ICommandFactory>()
                )
            );
            container.AddSingleton<IEventMediator, EventMediator>(
                provider => new EventMediator(
                    provider.GetService<IEventFactory>()
                )
            );
            container.AddSingleton<IQueryMediator, QueryMediator>(
                provider => new QueryMediator(
                    provider.GetService<IQueryFactory>()
                )
            );
            container.AddSingleton<IMediator, Mediator>(
                provider => new Mediator(
                    provider.GetService<ICommandMediator>(),
                    provider.GetService<IEventMediator>(),
                    provider.GetService<IQueryMediator>()
                )
            );

            return container;
        }

        private ServiceCollection AddLoggingToCollection(ServiceCollection container)
        {
            var logSettings = new LogSettings();
            logSettings.ConfigureLogger();
            container.AddLogging(configure => configure.AddSerilog(dispose: true));

            return container;
        }
    }
}