using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Hub;
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

    public class GetGridPathHandler : IQueryHandler<GetGridPathQuery>, IHasEnum
    {
        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetGridPath;
        }

        public Task<QueryResult> Handle(GetGridPathQuery query, CancellationToken cancellationToken = default)
        {
            var path = query._pathfindingService.GetPath(query.Origin, query.Target);

            return Task.FromResult(new QueryResult(
                (QueryTypeEnum)GetEnum(),
                true,
                path,
                typeof(Vector2[])
            ));
        }
    }
}