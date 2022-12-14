using System;
using System.Collections.Generic;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;
using static Modules.Global.ConvertEntityIdsToEntities;

namespace Modules.Combat
{
    public class UnitAddedToGridSystem : EcsSystem
    {
        private IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public UnitAddedToGridSystem(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public override void Update(float elapsedTime)
        {
            var unitIdsToAddResult = _mediator.RunQuery(QueryTypeEnum.GetUnitsToAddToGrid, null);
            var unitIdsToAdd = unitIdsToAddResult.Result.ConvertResultValue<List<int>>();

            if (unitIdsToAdd == null || unitIdsToAdd.Count <= 0)
                return;

            var unitEntitiesToAddResult = _mediator.RunQuery(
                QueryTypeEnum.ConvertEntityIdsToEntities,
                new ConvertEntityIdsToEntitiesQuery { EntityIds = unitIdsToAdd }
            );
            var unitEntitiesToAdd = unitEntitiesToAddResult.Result.ConvertResultValue<List<Entity>>();

            SendAddUnitIdToCellEntityCommand(unitEntitiesToAdd);
        }

        public void SendAddUnitIdToCellEntityCommand(List<Entity> entities)
        {
            var command = ActivatorUtilities.CreateInstance<AddUnitToGridCommand>(
                _serviceProvider,
                new object[] {
                    entities
                }
            );

            _mediator.ExecuteCommand(CommandTypeEnum.AddUnitIdToCellEntity, (ICommand)command);
        }
    }
}