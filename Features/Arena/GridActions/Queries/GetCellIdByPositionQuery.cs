using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Arena.GridActions
{
    public class GetCellIdByPositionQuery : IQuery
    {
        public Vector2 Position { get; set; }

        public GetCellIdByPositionQuery(Vector2 position)
        {
            Position = position;
        }
    }

    public class GetCellIdByPositionHandler : IQueryHandler<GetCellIdByPositionQuery>, IHasEnum
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetCellIdByPositionHandler(IEcsWorldService ecsWorldService)
        {
            _ecsWorldService = ecsWorldService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetCellIdByPosition;
        }

        public Task<QueryResult> Handle(GetCellIdByPositionQuery query, CancellationToken cancellationToken = default)
        {
            var arena = _ecsWorldService.GetWorld(EcsWorldEnum.Arena);

            var cells = arena.GetEntities().With<IsGridCell>().With<CurrentPosition>().AsSet().GetEntities();
            foreach (var cell in cells)
            {
                var cellPosition = cell.Get<CurrentPosition>().Value;
                if (cellPosition == query.Position)
                {
                    var cellId = cell.Get<EntityId>().Value;
                    var result = new QueryResult(
                        QueryTypeEnum.GetCellIdByPosition,
                        true,
                        cellId,
                        typeof(int)
                    );
                    return Task.FromResult(result);
                }
            }

            return default;
        }
    }
}