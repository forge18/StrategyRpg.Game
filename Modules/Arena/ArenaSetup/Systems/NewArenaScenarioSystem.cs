using System;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;

namespace Features.Arena.ArenaSetup
{
    public class NewArenaScenarioSystem : EcsSystem
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly World _world;

        public NewArenaScenarioSystem(
            IServiceProvider serviceProvider,
            IMediator mediator,
            IEcsWorldService ecsWorldService
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _world = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
        }

        public override async void Update(float elapsedTime)
        {
            var result = _mediator.RunQuery(QueryTypeEnum.GetArenaScenario, null);
            var scenario = result.Result.ConvertResultValue<string>();

            if (scenario == default)
                return;

            await SendCreateArenaCommand();
            await SendCreateGridCommand();
            await SendLoadArenaMapCommand();
            await SendSetObjectivesCommand();

            var mapEvents = await RunGetMapEventsQuery();
            await SendSetMapEventsCommand(mapEvents);

            var unitsToLoad = await RunGetUnitsToLoadQuery();
            foreach (var unit in unitsToLoad)
            {
                await SendAddUnitIdToCellEntityCommand();
            }
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

        public async Task<NoResult> SendLoadArenaMapCommand()
        {
            var command = ActivatorUtilities.CreateInstance<LoadArenaMapCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.LoadArenaMap, command);
        }

        public async Task<NoResult> SendAddUnitIdToCellEntityCommand()
        {
            var command = ActivatorUtilities.CreateInstance<AddUnitIdToCellEntityCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.AddUnitIdToCellEntity, command);
        }

        public async Task<NoResult> SendSetObjectivesCommand()
        {
            var command = ActivatorUtilities.CreateInstance<SetObjectivesCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.SetObjectives, command);
        }

        public async Task<NoResult> SendSetMapEventsCommand(EntitySet mapEvents)
        {
            var command = ActivatorUtilities.CreateInstance<SetMapEventsCommand>(
                _serviceProvider,
                new object[] {
                    _world
                }
            );
            return await _mediator.ExecuteCommand(CommandTypeEnum.SetMapEvents, command);
        }

        public async Task<EntitySet> RunGetMapEventsQuery()
        {
            var result = _mediator.RunQuery(QueryTypeEnum.GetMapEvents, null);
            return await Task.FromResult(result.Result.ConvertResultValue<EntitySet>());
        }

        public async Task<string[]> RunGetUnitsToLoadQuery()
        {
            var result = await _mediator.RunQuery(QueryTypeEnum.GetUnitsToLoad, null);
            var unitsToLoad = result.ConvertResultValue<string[]>();

            return unitsToLoad;
        }
    }
}