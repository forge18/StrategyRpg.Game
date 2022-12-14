using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Infrastructure.Pathfinding;

namespace Modules.Combat
{
    public class GetGridPathQuery : IQuery
    {
        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public GetGridPathQuery(Vector2 origin, Vector2 target)
        {
            Origin = origin;
            Target = target;
        }
    }

    public class GetGridPathHandler : IQueryHandler<GetGridPathQuery>, IHasEnum
    {
        public IPathfindingService _pathfindingService;

        public GetGridPathHandler(IPathfindingService pathfindingService)
        {
            _pathfindingService = pathfindingService;
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetGridPath;
        }

        public Task<QueryResult> Handle(GetGridPathQuery query, CancellationToken cancellationToken = default)
        {
            var path = _pathfindingService.GetPath(query.Origin, query.Target);

            return Task.FromResult(new QueryResult(
                (QueryTypeEnum)GetEnum(),
                true,
                path,
                typeof(Vector2[])
            ));
        }
    }
}