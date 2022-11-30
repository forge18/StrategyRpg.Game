using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;

namespace Features.Combat.GridActions
{
    public class GetCellIdByPositionQuery : IQuery
    {
        public Vector2 Position { get; set; }

        public GetCellIdByPositionQuery(Vector2 position)
        {
            Position = position;
        }
    }

    public class GetCellIdByPositionHandler : QueryHandler
    {
        private readonly IEcsWorldService _ecsWorldService;

        public GetCellIdByPositionHandler(IEcsWorldService ecsWorldService) : 
            base(ecsWorldService) 
        {
            _ecsWorldService = ecsWorldService;
        }

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetCellIdByPosition;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetCellIdByPositionQuery;
            var arena = _ecsWorldService.GetWorld("Arena");

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