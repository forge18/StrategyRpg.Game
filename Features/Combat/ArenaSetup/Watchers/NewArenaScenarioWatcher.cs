using System;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;

namespace Features.Combat.ArenaSetup
{
    public class NewArenaScenarioWatcher : Watcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly World _world;

        public NewArenaScenarioWatcher(
            IServiceProvider serviceProvider,
            IMediator mediator,
            IEcsWorldService ecsWorldService
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _world = ecsWorldService.GetWorld("arena");
        }

        public override async void Update(float elapsedTime)
        {
            var result = _mediator.RunQuery(QueryTypeEnum.GetArenaScenario, null);
            var scenario = result.ConvertResultValue<string>();

            if (scenario == default)
                return;

            await SendCreateArenaCommand();
            await SendCreateGridCommand();
        }

        public async Task<NoResult> SendCreateArenaCommand()
        {
            var command = ActivatorUtilities.CreateInstance<CreateArenaCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.CreateArena, command);
        }

        public async Task<NoResult> SendCreateGridCommand()
        {
            var command = ActivatorUtilities.CreateInstance<CreateGridCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.CreateGrid, command);
        }
    }
}