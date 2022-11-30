using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;

namespace Features.Combat.GridActions
{
    public class GetGridPathQuery : IQuery
    {
        public IPathfindingService _pathfindingService;

        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public GetGridPathQuery(IPathfindingService pathfindingService, Vector2 origin, Vector2 target)
        {
            _pathfindingService = pathfindingService;

            Origin = origin;
            Target = target;
        }
    }

    public class GetGridPathHandler : QueryHandler
    {
        public GetGridPathHandler(IEcsWorldService ecsWorldService) :
            base(ecsWorldService)
        {
        }

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetGridPath;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetGridPathQuery;

            var path = query._pathfindingService.GetPath(query.Origin, query.Target);

            return Task.FromResult(new QueryResult(
                GetEnum(),
                true,
                path,
                typeof(Vector2[])
            ));
        }
    }
}