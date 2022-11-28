using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;
using Infrastructure.Hub.QueryManagement.Dto;

namespace Infrastructure.Hub.QueryManagement.Queries
{
    public class GetPlayerEntityQuery : IQuery
    {

    }

    public class GetPlayerEntityHandler : QueryHandler
    {
        public GetPlayerEntityHandler(IEcsWorldService ecsWorldService) : 
            base(ecsWorldService) {}

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.GetPlayerEntity;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as GetPlayerEntityQuery;
            var playerEntityResult = _world.GetEntities().With<IsPlayerEntity>().AsSet().GetEntities();

            Entity playerEntity = playerEntityResult.Length > 0 ?
                playerEntityResult[0] : default;

            var result = new QueryResult(
                QueryTypeEnum.GetPlayerEntity,
                playerEntity != default,
                playerEntity,
                playerEntity != default ? typeof(DefaultEcs.Entity) : null
            );

            return Task.FromResult(result);
        }
    }
}