using System;
using Data;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Features.Exploration.Unit.Watchers;
using Features.Infrastructure.Watchers;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Systems;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Hub;
using Infrastructure.Hub.EventManagement;
using Infrastructure.Hub.EventManagement.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services;

namespace Infrastructure.DependencyInjection
{
    public class Startup
    {
        private ServiceProvider _container;

        private IServiceProvider _serviceProvider;
        private IEcsWorldService _ecsWorldService;
        private IEcsSystemService _ecsSystemService;
        private IEcsDataLoader _ecsDataLoader;
        private IEcsEntityService _ecsEntitiesService;
        private IHubMediator _mediator;
        private INodeLocatorService _nodeLocatorService;
        private ILoggerFactory _loggerFactory;
        

        private Node _gameRootNode;
        private DefaultParallelRunner _runner;
        private ISystem<float> _systems;

        public Startup(Node gameRootNode)
        {
            _gameRootNode = gameRootNode;
            _container = new ContainerBuilder().Build();

            LoadRequiredServices();
            Run();
            LoadTestData();
            RegisterWatchers();
        }

        public void LoadRequiredServices()
        {
            _serviceProvider = _container.GetService<IServiceProvider>();
            _ecsWorldService = _container.GetService<IEcsWorldService>();
            _ecsSystemService = _container.GetService<IEcsSystemService>();
            _ecsEntitiesService = _container.GetService<IEcsEntityService>();
            _ecsDataLoader = _container.GetService<IEcsDataLoader>();
            _loggerFactory = _container.GetService<ILoggerFactory>();
            _mediator = _container.GetService<IHubMediator>();
            _nodeLocatorService = _container.GetService<INodeLocatorService>();

            var gameEvent = new EcsSystemsLoadedEvent(_systems);
            _mediator.NotifyOfEvent(EventTypeEnum.EcsSystemsLoaded, gameEvent);
        }

        public void Run()
        {
            new Bootstrapper(
                _mediator, 
                _ecsSystemService, 
                _nodeLocatorService, 
                _loggerFactory,
                _gameRootNode
            ).Run();
        }

        public void RegisterWatchers()
        {
            _runner = new DefaultParallelRunner(System.Environment.ProcessorCount);
            _systems = new SequentialSystem<float>(
                new ProcessInputWatcher(_ecsWorldService, _mediator),
                new PlayerVelocityWatcher(_serviceProvider, _mediator, _nodeLocatorService, _ecsWorldService, _ecsEntitiesService),
                new SpawnUnitWatcher(_serviceProvider, _mediator)
            );

            _ecsSystemService.RegisterSystems(_systems);
        }

        private void LoadTestData()
        {
            var newEntity = _ecsDataLoader.LoadResource(SchemaTypeEnum.Unit, "Godette");
            newEntity.Set<CurrentPosition>(new CurrentPosition { Value = new Vector2(0, 0) });
            newEntity.Set<NeedToRender>();
            newEntity.Set<IsPlayerEntity>();
        }

    }
}