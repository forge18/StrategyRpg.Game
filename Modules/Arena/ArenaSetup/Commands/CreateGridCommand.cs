using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;
using Infrastructure.Hub;

namespace Features.Arena.ArenaSetup
{
    public class CreateGridCommand : ICommand
    {
        public Vector2 GridDimensions { get; set; }

        public CreateGridCommand(Vector2 gridDimensions)
        {
            GridDimensions = gridDimensions;
        }
    }

    public class CreateGridHandler : ICommandHandler<CreateGridCommand>, IHasEnum
    {
        private readonly IEcsEntityService _ecsEntityService;
        private readonly IPathfindingService _pathfindingService;
        private World _arena;

        public CreateGridHandler(
            IEcsEntityService ecsEntityService,
            IPathfindingService pathfindingService,
            IEcsWorldService ecsWorldService
        )
        {
            _ecsEntityService = ecsEntityService;
            _pathfindingService = pathfindingService;
            _arena = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.CreateGrid;
        }

        public Task Handle(CreateGridCommand command, CancellationToken cancellationToken = default)
        {
            for (int x = 0; x < command.GridDimensions.x; x++)
            {
                for (int y = 0; y < command.GridDimensions.y; y++)
                {
                    CreateGridCell(command, new Vector2(x, y));
                }
            }

            CreateGridCellConnections(command);

            return Task.CompletedTask;
        }

        private void CreateGridCell(CreateGridCommand command, Vector2 coordinates)
        {
            var cellEntity = _ecsEntityService.CreateEntityInWorld(EcsWorldEnum.Arena);
            var cellEntityId = _ecsEntityService.ParseEntityId(cellEntity);
            cellEntity.Set<EntityId>(new EntityId() { Value = cellEntityId } );
            cellEntity.Set<IsGridCell>();
            _pathfindingService.AddCell(cellEntityId, coordinates);
        }

        private void CreateGridCellConnections(CreateGridCommand command)
        {
            var gridCells = _arena.GetEntities().With<IsGridCell>().AsSet();
            foreach (var gridCell in gridCells.GetEntities())
            {
                var gridCellPosition = gridCell.Get<CurrentPosition>().Value;
                var gridCellId = _pathfindingService.GetCellIdByPosition(gridCellPosition);

                var neighborPositions = new Vector2[4];
                neighborPositions[0] = Vector2.Up;
                neighborPositions[1] = Vector2.Down;
                neighborPositions[2] = Vector2.Left;
                neighborPositions[3] = Vector2.Right;

                var index = 0;
                var neighborCells = new Vector2[4];
                foreach (var neighbor in neighborPositions) 
                {
                    var cell = gridCellPosition + neighbor;
                    var isOutOfBounds = IsPositionOutOfBounds(command, cell);
                    if (isOutOfBounds)
                    {
                        continue;
                    }

                    _pathfindingService.ConnectCells(
                        gridCellId, 
                        _pathfindingService.GetCellIdByPosition(cell)
                    );
                    neighborCells[index] = cell;
                    index++;
                }
                gridCell.Set<NeighborCells>(new NeighborCells() { Values = neighborCells });
            }
        }

        private bool IsPositionOutOfBounds(CreateGridCommand command, Vector2 cell)
        {
            return (
                cell.x < 0 ||
                cell.y < 0 ||
                cell.x > cell.x + command.GridDimensions.x ||
                cell.y > cell.y + command.GridDimensions.y
            );
        }
    }
}