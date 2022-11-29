using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;

namespace Features.Combat.ArenaSetup
{
    public class CreateGridCommand : ICommand
    {
        public readonly IEcsEntityService _ecsEntityService;
        public readonly IPathfindingService _pathfindingService;

        public World Arena { get; set;}
        public Vector2 OriginCoordinates { get; set; }
        public Vector2 GridDimensions { get; set; }

        public CreateGridCommand(IEcsEntityService ecsEntityService, IPathfindingService pathfindingService, World world, Vector2 originCoordinates, Vector2 gridDimensions)
        {
            _ecsEntityService = ecsEntityService;
            _pathfindingService = pathfindingService;

            Arena = world;
            OriginCoordinates = originCoordinates;
            GridDimensions = gridDimensions;
        }
    }

    public class CreateGridHandler : ICommandHandler
    {
        public CommandTypeEnum GetEnum()
        {
            return CommandTypeEnum.CreateGrid;
        }

        public Task Handle(ICommand genericCommand, CancellationToken cancellationToken = default)
        {
            var command = genericCommand as CreateGridCommand;

            for (int x = 0; x < command.GridDimensions.x; x++)
            {
                for (int y = 0; y < command.GridDimensions.y; y++)
                {
                    var offsetX = command.OriginCoordinates.x + x;
                    var offsetY = command.OriginCoordinates.y + y;
                    CreateGridCell(command, new Vector2(offsetX, offsetY));
                }
            }

            CreateGridCellConnections(command);

            return Task.CompletedTask;
        }

        private void CreateGridCell(CreateGridCommand command, Vector2 coordinates)
        {
            var cellEntity = command._ecsEntityService.CreateEntityInWorld("Arena");
            var cellEntityId = command._ecsEntityService.ParseEntityId(cellEntity);
            cellEntity.Set<EntityId>(new EntityId() { Value = cellEntityId } );
            cellEntity.Set<IsGridCell>();
            cellEntity.Set<CurrentPosition>(new CurrentPosition() { Value = command.OriginCoordinates });
            command._pathfindingService.AddCell(cellEntityId, coordinates);
        }

        private void CreateGridCellConnections(CreateGridCommand command)
        {
            var gridCells = command.Arena.GetEntities().With<IsGridCell>().AsSet();
            foreach (var gridCell in gridCells.GetEntities())
            {
                var gridCellPosition = gridCell.Get<CurrentPosition>().Value;
                var gridCellId = command._pathfindingService.GetCellIdByPosition(gridCellPosition);

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

                    command._pathfindingService.ConnectCells(
                        gridCellId, 
                        command._pathfindingService.GetCellIdByPosition(cell)
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
                (cell.x - command.OriginCoordinates.x) < 0 ||
                (cell.y - command.OriginCoordinates.y) < 0 ||
                (cell.x - command.OriginCoordinates.x) > cell.x + command.GridDimensions.x ||
                (cell.y - command.OriginCoordinates.y) > cell.y + command.GridDimensions.y
            );
        }
    }
}