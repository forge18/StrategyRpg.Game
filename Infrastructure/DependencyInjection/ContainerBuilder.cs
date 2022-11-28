using System;
using Data;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Systems;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Logging;
using Infrastructure.Hub;
using Infrastructure.Hub.CommandManagement;
using Infrastructure.Hub.EventManagement;
using Infrastructure.Hub.QueryManagement;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;
using Serilog;
using Microsoft.Extensions.Logging;

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
					provider.GetService<IHubMediator>(),
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
			container.AddSingleton<IHubMediator, HubMediator>(
				provider => new HubMediator(
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
