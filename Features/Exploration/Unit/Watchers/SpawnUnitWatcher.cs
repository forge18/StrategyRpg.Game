using System;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;
using Infrastructure.MediatorNS;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.QueryManagement;
using Microsoft.Extensions.DependencyInjection;
using StrategyRpg.Game.Features.Exploration.Unit.Commands.RenderUnit;

namespace Features.Exploration.Unit.Watchers
{
    public class SpawnUnitWatcher : Watcher
    {
        private IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IEcsQueryService _ecsQueryService;

        public SpawnUnitWatcher(IServiceProvider serviceProvider, IMediator mediator, IEcsQueryService ecsQueryService)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _ecsQueryService = ecsQueryService;
        }

        public override void Update(float elapsedTime)
        {
            var query = new EmptyQuery();
            var entitiesToRenderResult = _mediator.RunQuery(QueryTypeEnum.GetEntitiesToRender, query);
            var entitiesToRender = entitiesToRenderResult.ConvertResultValue<Entity[]>();

            if (entitiesToRender == null)
                return;

            var processId = Guid.NewGuid();
            foreach (var entity in entitiesToRender)
            {
                var unitTypeId = entity.Get<UnitType>();
                var unitTypeEntity = _ecsQueryService.GetEntityBySchemaId(unitTypeId.Value);
                if (unitTypeEntity.Has<Sprite>())
                {
                    SendRenderUnitCommand(processId, entity, unitTypeEntity);
                }
            }
        }

        public void SendRenderUnitCommand(Guid processId, Entity entity, Entity unitTypeEntity)
        {
            var command = ActivatorUtilities.CreateInstance<SpawnUnitCommand>(
                _serviceProvider,
                new object[] {
                    processId,
                    entity,
                    unitTypeEntity
                }
            );

            _mediator.ExecuteCommand(CommandTypeEnum.SpawnUnit, (ICommand)command);
        }
    }
}