using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;

namespace Infrastructure.HubMediator
{
    public class PlayerIsMovingQuery : IQuery
    {

    }

    public class PlayerIsMovingHandler : QueryHandler
    {
        public PlayerIsMovingHandler(IEcsWorldService ecsWorldService, ILoggerFactory loggerFactory) 
            : base(ecsWorldService) {}

        public override QueryTypeEnum GetEnum()
        {
            return QueryTypeEnum.PlayerIsMoving;
        }

        public override Task<QueryResult> Handle(IQuery genericQuery, CancellationToken cancellationToken = default)
        {
            var query = genericQuery as PlayerIsMovingQuery;
            var playerEntity = _world.GetEntities().With<IsPlayerEntity>().AsSet().GetEntities()[0];

            var playerHasVelocity = PlayerHasVelocity(playerEntity);

            var result = CreateResultObject(
                QueryTypeEnum.PlayerIsMoving,
                true,
                playerHasVelocity,
                playerHasVelocity.GetType()
            );

            return Task.FromResult(result);
        }

        public bool PlayerHasVelocity(Entity playerEntity)
        {
            var playerIsOnBoard = playerEntity.Has<CurrentPosition>();
            var playerHasMoveSpeed = playerEntity.Has<MoveSpeed>();
            var playerInputExists = _world.Has<DownKey>() || _world.Has<UpKey>() || _world.Has<LeftKey>() || _world.Has<RightKey>();

            return playerIsOnBoard && playerHasMoveSpeed && playerInputExists;
        }
    }
}