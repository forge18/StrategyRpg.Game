using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Data;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Logging;
using Infrastructure.Pathfinding;
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
			container = AddMediatorToCollection(container);
			container = AddLoggingToCollection(container);

			return container.BuildServiceProvider();
		}

		public ServiceCollection AddServicesToCollection(ServiceCollection container)
		{
			container.AddTransient<IEcsDataLoader, EcsDataLoader>(
				provider => new EcsDataLoader(
					provider.GetService<IEcsEntityService>()
				)
			);
			container.AddSingleton<IEcsEntityService, EcsEntityService>(
				provider => new EcsEntityService(
					provider.GetService<IMediator>(),
					provider.GetService<IEcsWorldService>()
				)
			);
			container.AddSingleton<IEcsSystemService, EcsSystemService>();
			container.AddSingleton<IEcsWorldService, EcsWorldService>();
			container.AddSingleton<INodeLocatorService, NodeLocatorService>();
			container.AddSingleton<INodeService, NodeService>(
				provider => new NodeService(
					provider.GetService<INodeLocatorService>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IPathfindingService, PathfindingService>();
			container.AddSingleton<IServiceProvider, ServiceProvider>();

			return container;
		}

		public ServiceCollection AddFactoriesToCollection(ServiceCollection container)
		{
			container.AddTransient<ICommandFactory, CommandFactory>(
				provider => new CommandFactory(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddTransient<IEventFactory, EventFactory>(
				provider => new EventFactory(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddTransient<IQueryFactory, QueryFactory>(
				provider => new QueryFactory(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);

			return container;
		}

		public ServiceCollection AddMediatorToCollection(ServiceCollection container)
		{
			container.AddSingleton<ICommandMediator, CommandMediator>(
				provider => new CommandMediator(
					provider.GetService<ICommandFactory>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IEventMediator, EventMediator>(
				provider => new EventMediator(
					provider.GetService<IEventFactory>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IQueryMediator, QueryMediator>(
				provider => new QueryMediator(
					provider.GetService<IQueryFactory>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IMediator, Mediator>(
				provider => new Mediator(
					provider.GetService<ICommandMediator>(),
					provider.GetService<IEventMediator>(),
					provider.GetService<IQueryMediator>(),
					provider.GetService<ILoggerFactory>()
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
