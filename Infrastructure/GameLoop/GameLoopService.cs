using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Infrastructure.Ecs;
using Infrastructure.GameLoop;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;
using StronglyConnectedComponents;

namespace Infrastructure
{
    public class GameLoopService : IGameLoopService
    {
        private IServiceProvider _serviceProvider;
        private IMediator _mediator;
        private IEcsWorldService _ecsWorldService;
        private INodeLocatorService _nodeLocatorService;

        private SequentialSystem<float> _systems;
        private ISystem<float>[] _watchersToRun;
        private List<WatcherRegistration> _watchers;
        private readonly DefaultParallelRunner _runner;

        public GameLoopService(
            IServiceProvider serviceProvider,
            IMediator mediator,
            IEcsWorldService ecsWorldService,
            INodeLocatorService nodeLocatorService,
            ISystem<float> watchers
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _ecsWorldService = ecsWorldService;
            _nodeLocatorService = nodeLocatorService;
            _runner = new DefaultParallelRunner(System.Environment.ProcessorCount);

            BuildWatcherSequence();
        }

        public void BuildWatcherSequence()
        {
            GetWatchers();
            SortWatchers();
            _systems = new SequentialSystem<float>(_watchersToRun);
        }

        private void GetWatchers()
        {
            var types = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(s => s.GetType())
                .Where(p => typeof(IGameLoop).IsAssignableFrom(p));

            var watchers = new List<WatcherRegistration>();
            foreach (var group in types)
            {
                var groupInstance = (IGameLoop)ActivatorUtilities.CreateInstance(_serviceProvider, group);
                var groupWatchers = groupInstance.GetWatchers();

                foreach (var watcher in groupWatchers)
                {
                    watchers.Add(watcher);
                }
            }

            _watchers = watchers;
        }

        private void SortWatchers()
        {
            var sortedWatchers= _watchers.DetectCyclesUsingKey(
                s => s.Value,
                s => s.DependsOn
            );

            foreach (var sortedWatcher in sortedWatchers)
            {
                var index = 0;
                var unwrappedWatchers = sortedWatcher.Contents;
                foreach (var item in unwrappedWatchers)
                {
                    var watcherInstance = (ISystem<float>)ActivatorUtilities.CreateInstance(_serviceProvider, item.Value);
                    _watchersToRun[index] = watcherInstance;
                    index++;
                }
            }
        }
    }
}