using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Data;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Features.Exploration.Unit;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Presentation.Services;
using Features.Global;

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
        private IMediator _mediator;
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
            _mediator = _container.GetService<IMediator>();
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