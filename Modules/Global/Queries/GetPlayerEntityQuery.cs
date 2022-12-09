using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class GetPlayerEntityQuery : IQuery
    {

    }

    public class GetPlayerEntityHandler : IQueryHandler<GetPlayerEntityQuery>, IHasEnum
    {
        private readonly World _world;

        public GetPlayerEntityHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.GetPlayerEntity;
        }

        public Task<QueryResult> Handle(GetPlayerEntityQuery query, CancellationToken cancellationToken = default)
        {
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