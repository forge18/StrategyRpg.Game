using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;

namespace Infrastructure.MediatorNS.QueryManagement.Queries
{
    public class GetEntitiesToRenderQuery : IQuery
    {

    }

    public class GetEntitiesToRenderHandler : IQueryHandler<GetEntitiesToRenderQuery>
    {
        private readonly World _world;

        public GetEntitiesToRenderHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public Task<QueryResult> Handle(GetEntitiesToRenderQuery query, CancellationToken cancellationToken = default)
        {
            var entities = _world.GetEntities().With<NeedToRender>().AsSet();
            var result = new QueryResult(
                QueryTypeEnum.GetEntitiesToRender,
                true,
                entities,
                entities.GetType()
            );

            return Task.FromResult(result);
        }
    }
}