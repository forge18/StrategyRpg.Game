using System;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Features.Exploration.Unit.Watchers;
using Features.Infrastructure.Watchers;
using Godot;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;
using Infrastructure.Ecs.Worlds;
using Infrastructure.MediatorNS;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;

namespace Infrastructure.DependencyInjection
{
    public class Startup
    {
        private static readonly ServiceProvider _container = new ContainerBuilder().Build();

        private IServiceProvider _serviceProvider;
        private IEcsEntityService _ecsEntitiesService;
        private IEcsQueryService _ecsQueryService;
        private IEcsWorldService _ecsWorldService;
        private IMediator _mediator;
        private INodeLocatorService _nodeLocatorService;

        private Node _gameRootNode;
        private DefaultParallelRunner _runner;
        private ISystem<float> _systems;

        public Startup(Node gameRootNode)
        {
            _gameRootNode = gameRootNode;

            LoadRequiredServices();
            RegisterWatchers();
        }

        public void LoadRequiredServices()
        {
            _serviceProvider = _container.GetService<IServiceProvider>();
            _ecsEntitiesService = _serviceProvider.GetService<IEcsEntityService>();
            _ecsQueryService = _serviceProvider.GetService<IEcsQueryService>();
            _ecsWorldService = _serviceProvider.GetService<IEcsWorldService>();
            _mediator = _serviceProvider.GetService<IMediator>();
            _nodeLocatorService = _serviceProvider.GetService<INodeLocatorService>();
        }

        public void Run()
        {
            new Bootstrapper(_mediator, _nodeLocatorService, _gameRootNode).Run();
        }

        public void RegisterWatchers()
        {
            _runner = new DefaultParallelRunner(System.Environment.ProcessorCount);
            _systems = new SequentialSystem<float>(
                new ProcessInputWatcher(_ecsWorldService, _mediator),
                new PlayerVelocityWatcher(_serviceProvider, _mediator, _nodeLocatorService, _ecsWorldService, _ecsEntitiesService),
                new SpawnUnitWatcher(_serviceProvider, _mediator, _ecsQueryService)
            );
        }

    }
}