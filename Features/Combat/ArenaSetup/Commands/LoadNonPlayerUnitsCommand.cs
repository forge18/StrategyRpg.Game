using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;
using Features.Global;

namespace Features.Combat.ArenaSetup
{
    public class LoadNonPlayerUnitsCommand : ICommand
    {
        public readonly World Arena;
        public readonly IMediator _mediator;
        public readonly IEcsEntityService _ecsEntityService;
        public readonly IPathfindingService _pathfindingService;

        public Collection<KeyValuePair<Vector2,Entity>> NonPlayerUnits;

        public LoadNonPlayerUnitsCommand(
            IEcsWorldService ecsWorldService,
            IMediator mediator,
            IEcsEntityService ecsEntityService,
            IPathfindingService pathfindingService,
            Collection<KeyValuePair<Vector2,Entity>> nonPlayerUnits
        )
        {
            Arena = ecsWorldService.GetWorld("Arena");
            _mediator = mediator;
            _ecsEntityService = ecsEntityService;
            _pathfindingService = pathfindingService;
            NonPlayerUnits = nonPlayerUnits;
        }
    }

    public class LoadNonPlayerUnitsHandler : ICommandHandler
    {
        public CommandTypeEnum GetEnum()
        {
            return CommandTypeEnum.LoadNonPlayerUnits;
        }

        public Task Handle(ICommand genericCommand, CancellationToken cancellationToken = default)
        {
            var command = genericCommand as LoadNonPlayerUnitsCommand;
            
            foreach (var (cell, unitEntity) in command.NonPlayerUnits)
            {
                var cellEntity = GetCellEntity(command, cell);
                AddUnitIdToCellEntity(command, cellEntity, unitEntity);
            }

            return Task.CompletedTask;
        }   

        private Entity GetCellEntity(LoadNonPlayerUnitsCommand command, Vector2 cell)
        {
            var cellId = command._pathfindingService.GetCellIdByPosition(cell);
            var cellEntityResult = command._mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(
                    "Arena",
                    cellId
                )
            );
            return cellEntityResult.ConvertResultValue<Entity>();
        }

        private void AddUnitIdToCellEntity(
            LoadNonPlayerUnitsCommand command,
            Entity cellEntity,
            Entity unitEntity
        )
        {
            if (cellEntity.Has<UnitInCell>())
            {
                GD.Print("Cell already has unit");
                return;
            }

            cellEntity.Set<UnitInCell>(
                new UnitInCell(){ 
                    UnitId = command._ecsEntityService.ParseEntityId(unitEntity) 
                }
            );
        }
        
    }
}