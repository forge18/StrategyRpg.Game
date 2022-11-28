using System;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.Hub.CommandManagement;
using Infrastructure.Hub.QueryManagement;
using Microsoft.Extensions.DependencyInjection;
using Features.Exploration.Unit.Commands.RenderUnit;
using Infrastructure.Hub.QueryManagement.QueriesWithParams;

namespace Features.Exploration.Unit.Watchers
{
    public class SpawnUnitWatcher : Watcher
    {
        private IServiceProvider _serviceProvider;
        private readonly IHubMediator _mediator;

        public SpawnUnitWatcher(IServiceProvider serviceProvider, IHubMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public override void Update(float elapsedTime)
        {
            var entitiesToRenderResult = _mediator.RunQuery(QueryTypeEnum.GetEntitiesToRender, null);
            var entitiesToRender = entitiesToRenderResult.ConvertResultValue<EntitySet>();

            if (entitiesToRender == null || entitiesToRender.Count <= 0)
                return;

            var processId = Guid.NewGuid();
            foreach (var entity in entitiesToRender.GetEntities())
            {
                var unitTypeId = entity.Get<UnitType>();
                var result = _mediator.RunQuery(
                    QueryTypeEnum.GetEntityBySchemaId, 
                    new GetEntityBySchemaIdQuery { SchemaId = unitTypeId.Value }
                );
                var unitTypeEntity = result.ConvertResultValue<Entity>();
                if (unitTypeEntity.Has<Sprite>())
                {
                    SendRenderUnitCommand(processId, entity, unitTypeEntity);
                    entity.Remove<NeedToRender>();
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