using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Data;
using Infrastructure.HubMediator;
using Infrastructure.Logging;
using Infrastructure.Pathfinding;
using Presentation.Services;
using Serilog;
using System.Linq;
using Infrastructure.Ecs;

namespace Infrastructure.DependencyInjection
{
	public class ContainerBuilder
	{
		public ServiceProvider Build()
		{
			var container = new ServiceCollection();

			container = AddServicesToCollection(container);
			container = AddMediatorToCollection(container);
			container = AddCommandsToCollection(container);
			container = AddEventsToCollection(container);
			container = AddQueriesToCollection(container);
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

		public ServiceCollection AddMediatorToCollection(ServiceCollection container)
		{
			container.AddSingleton<ICommandProcessor, CommandProcessor>(
				provider => new CommandProcessor(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IEventProcessor, EventProcessor>(
				provider => new EventProcessor(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IQueryProcessor, QueryProcessor>(
				provider => new QueryProcessor(
					provider.GetService<IServiceProvider>(),
					provider.GetService<ILoggerFactory>()
				)
			);
			container.AddSingleton<IMediator, Mediator>(
				provider => new Mediator(
					provider.GetService<ICommandProcessor>(),
					provider.GetService<IEventProcessor>(),
					provider.GetService<IQueryProcessor>(),
					provider.GetService<ILoggerFactory>()
				)
			);

			return container;
		}

		public ServiceCollection AddCommandsToCollection(ServiceCollection container)
		{
			var commandHandlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
					)
				);

			foreach (var handler in commandHandlers)
			{
				container.AddScoped(
					handler.GetInterfaces()
					.First(i => 
						i.IsGenericType && 
						i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
					), handler
				);
			}

			return container;
		}

		public ServiceCollection AddEventsToCollection(ServiceCollection container)
		{
			var eventHandlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(IEventHandler<>)
					)
				);

			foreach (var handler in eventHandlers)  
			{  
				if (handler.Name == "EventHandler`1")  
				{  
					continue;  
				}

				container.AddScoped(
					handler.GetInterfaces()
					.First(i => 
						i.IsGenericType && 
						i.Name != "EventHandler`1" &&
						i.GetGenericTypeDefinition() == typeof(IEventHandler<>)
					), handler
				);
			}  

			return container;
		}

		public ServiceCollection AddQueriesToCollection(ServiceCollection container)
		{
			var queryHandlers = typeof(Startup).Assembly.GetTypes()
            	.Where(t =>
					t.GetInterfaces()
					.Any(i =>
						i.IsGenericType &&
						!i.ContainsGenericParameters &&
						i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
					)
				);

			foreach (var handler in queryHandlers)  
			{  
				container.AddScoped(
					handler.GetInterfaces()
					.First(i => 
						i.IsGenericType && 
						!i.ContainsGenericParameters &&
						i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
					), handler
				);
			}  

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
