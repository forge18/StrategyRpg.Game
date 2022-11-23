using System.Reflection;
using Data;
using Infrastructure.Commands;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;
using Infrastructure.Ecs.Systems;
using Infrastructure.Ecs.Worlds;
using Infrastructure.EventManagement;
using Infrastructure.Logging;
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Events.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
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
            container = AddLiteBusToCollection(container);
            container = AddLoggingToCollection(container);

            return container.BuildServiceProvider();
        }

        public ServiceCollection AddServicesToCollection(ServiceCollection container)
        {
            container.AddSingleton<ICommandService, CommandService>();
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
            container.AddSingleton<IEventService, EventService>();
            container.AddSingleton<INodeLocatorService, NodeLocatorService>();
            container.AddSingleton<INodeService, NodeService>();


            return container;
        }

        private ServiceCollection AddLiteBusToCollection(ServiceCollection container)
        {
            container.AddLiteBus(builder =>
                builder
                    .AddCommands(commandBuilder =>
                    {
                        commandBuilder.RegisterFrom(typeof(Game).Assembly);
                    })
                    .AddEvents(eventBuilder =>
                    {
                        eventBuilder.RegisterFrom(typeof(Game).Assembly);
                    })
                    .AddQueries(queryBuilder =>
                    {
                        queryBuilder.RegisterFrom(typeof(Game).Assembly);
                    })
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