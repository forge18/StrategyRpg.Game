using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;
using Infrastructure.MediatorNS.QueryManagement;

namespace Infrastructure.Ecs.Queries.StaticQueries
{
    public class GetPlayerEntityQuery : IQuery
    {

    }

    public class GetPlayerEntityHandler : IQueryHandler<GetPlayerEntityQuery>
    {
        private readonly World _world;

        public GetPlayerEntityHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public Task<QueryResult> Handle(GetPlayerEntityQuery query, CancellationToken cancellationToken = default)
        {
            var playerEntity = _world.GetEntities().With<IsPlayerEntity>().AsSet().GetEntities()[0];
            var result = new QueryResult(
                QueryTypeEnum.GetPlayerEntity,
                true,
                playerEntity,
                playerEntity.GetType()
            );

            return Task.FromResult(result);
        }
    }
}