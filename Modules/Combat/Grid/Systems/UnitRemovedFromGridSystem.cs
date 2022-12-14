using System;
using System.Collections.Generic;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;
using static Modules.Global.ConvertEntityIdsToEntities;

namespace Modules.Combat
{
    public class UnitRemovedFromGridSystem : EcsSystem
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public UnitRemovedFromGridSystem(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public override void Update(float elapsedTime)
        {
            var unitIdsToMoveResult = _mediator.RunQuery(QueryTypeEnum.GetUnitsToRemoveFromGrid, null);
            var unitIdsToRemove = unitIdsToMoveResult.Result.ConvertResultValue<List<int>>();

            if (unitIdsToRemove == null || unitIdsToRemove.Count <= 0)
                return;

            var unitEntitiesToMoveResult = _mediator.RunQuery(
                QueryTypeEnum.ConvertEntityIdsToEntities,
                new ConvertEntityIdsToEntitiesQuery { EntityIds = unitIdsToRemove }
            );
            var unitEntitiesToMove = unitEntitiesToMoveResult.Result.ConvertResultValue<List<Entity>>();

            foreach (var entity in unitEntitiesToMove)
            {
                SendRemoveUnitFromGridCommand(entity);
            }
        }

        public void SendRemoveUnitFromGridCommand(Entity entity)
        {
            var command = ActivatorUtilities.CreateInstance<RemoveUnitFromGridCommand>(
                _serviceProvider,
                new object[] {
                    entity
                }
            );

            _mediator.ExecuteCommand(CommandTypeEnum.RemoveUnitFromGrid, (ICommand)command);
        }
    }
}