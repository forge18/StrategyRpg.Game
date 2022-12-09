using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Features.Global;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;

namespace Features.Arena.ArenaSetup
{
    public class GetCellEntityByCellIdQuery : IQuery
    {
        public Vector2 CellPosition { get; set; }

        public GetCellEntityByCellIdQuery(
            Vector2 cellPosition
        )
        {
            CellPosition = cellPosition;
        }

    }

    public class GetCellEntityByCellIdHandler : IQueryHandler<GetCellEntityByCellIdQuery>, IHasEnum
    {
        public readonly World _world;
        public readonly IPathfindingService _pathfindingService;

        public GetCellEntityByCellIdHandler(
            IEcsWorldService ecsWorldService,
            IPathfindingService pathfindingService
        )
        {
            _world = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            _pathfindingService = pathfindingService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetCellEntityByCellId;
        }

        public Task<QueryResult> Handle(GetCellEntityByCellIdQuery query, CancellationToken cancellationToken = default)
        {
            var cellId = _pathfindingService.GetCellIdByPosition(query.CellPosition);
            var result = _world.GetEntities().With<EntityId>().With<IsGridCell>().AsSet().GetEntities();

            dynamic cellEntity = result.Length > 0 ? result[0] : default;
            return Task.FromResult(
                new QueryResult(
                    QueryTypeEnum.GetCellEntityByCellId,
                    result.Length > 0,
                    cellEntity,
                    typeof(Entity)
                )
            );
        }
    }
}