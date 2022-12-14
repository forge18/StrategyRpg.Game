using System;
using Data;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
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
        private IMediator _mediator;
        private INodeTreeService _nodeTreeService;
        private ILoggerFactory _loggerFactory;


        private Node _gameRootNode;
        private DefaultParallelRunner _runner;
        private ISystem<float> _systems;

        private static Boolean _isRunning = false;

        public Startup(Node gameRootNode)
        {
            _gameRootNode = gameRootNode;
            _container = new ContainerBuilder().Build();

            Run();
        }

        public void Run()
        {
            if (_isRunning)
            {
                return;
            }

            LoadRequiredServices();
            RunBootstrapper();
            LoadTestData();

            _isRunning = true;
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
            _nodeTreeService = _container.GetService<INodeTreeService>();
        }

        private void RunBootstrapper()
        {
            new Bootstrapper(
                _mediator,
                _ecsSystemService,
                _nodeTreeService,
                _loggerFactory,
                _gameRootNode
            ).Run();
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