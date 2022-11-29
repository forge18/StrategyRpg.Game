using System;
using Microsoft.Extensions.DependencyInjection;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;

namespace Features.Exploration.Unit
{
    public class SpawnUnitWatcher : Watcher
    {
        private IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public SpawnUnitWatcher(IServiceProvider serviceProvider, IMediator mediator)
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