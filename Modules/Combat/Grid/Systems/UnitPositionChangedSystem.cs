using System;
using System.Collections.Generic;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;
using static Modules.Global.ConvertEntityIdsToEntities;

namespace Modules.Combat
{
    public class UnitPositionChangedSystem : EcsSystem
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public UnitPositionChangedSystem(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public override void Update(float elapsedTime)
        {
            var unitIdsToMoveResult = _mediator.RunQuery(QueryTypeEnum.GetUnitsToMoveOnGrid, null);
            var unitIdsToMove = unitIdsToMoveResult.Result.ConvertResultValue<List<int>>();

            if (unitIdsToMove == null || unitIdsToMove.Count <= 0)
                return;

            var unitEntitiesToMoveResult = _mediator.RunQuery(
                QueryTypeEnum.ConvertEntityIdsToEntities,
                new ConvertEntityIdsToEntitiesQuery { EntityIds = unitIdsToMove }
            );
            var unitEntitiesToMove = unitEntitiesToMoveResult.Result.ConvertResultValue<List<Entity>>();

            foreach (var entity in unitEntitiesToMove)
            {
                SendMoveUnitOnGridCommand(entity);
            }
        }

        public void SendMoveUnitOnGridCommand(Entity entity)
        {
            var command = ActivatorUtilities.CreateInstance<MoveUnitOnGridCommand>(
                _serviceProvider,
                new object[] {
                    entity
                }
            );

            _mediator.ExecuteCommand(CommandTypeEnum.MoveUnitOnGrid, (ICommand)command);
        }
    }
}