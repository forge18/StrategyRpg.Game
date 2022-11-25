using Data;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;
using Infrastructure.Ecs.Systems;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Logging;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.EventManagement;
using Infrastructure.MediatorNS.QueryManagement;
using Infrastructure.MediatorNS.QueryManagement.Queries;
using MediatR;
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
            container.AddSingleton<IEcsEntityService, EcsEntityService>(
                provider => new EcsEntityService(
                    provider.GetService<IEcsWorldService>(),
                    provider.GetService<IEcsQueryService>()
                )
            );
            container.AddTransient<IEcsDataLoader, EcsDataLoader>(
                provider => new EcsDataLoader(
                    provider.GetService<IEcsEntityService>(),
                    provider.GetService<IEcsQueryService>()
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
            container.AddSingleton<INodeService, NodeService>();

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
            container.AddTransient<IQueryHandler<GetEntitiesToRenderQuery>, GetEntitiesToRenderHandler>();

            return container;
        }

        public ServiceCollection AddMediatorToCollection(ServiceCollection container)
        {
            container.AddSingleton<ICommandMediator, CommandMediator>();
            container.AddSingleton<IEventMediator, EventMediator>();
            container.AddSingleton<IQueryMediator, QueryMediator>();
            container.AddSingleton<IMediator, Mediator>();

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