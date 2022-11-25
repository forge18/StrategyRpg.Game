using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;
using Infrastructure.MediatorNS.QueryManagement;

namespace StrategyRpg.Game.Infrastructure.Mediator.QueryManagement.Queries
{
    public class PlayerIsMovingQuery : IQuery
    {

    }

    public class PlayerIsMovingHandler : IQueryHandler<PlayerIsMovingQuery>
    {
        private readonly World _world;

        public PlayerIsMovingHandler(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public Task<QueryResult> Handle(PlayerIsMovingQuery query, CancellationToken cancellationToken = default)
        {
            var playerEntity = _world.GetEntities().With<IsPlayerEntity>().AsSet().GetEntities()[0];

            var playerIsOnBoard = playerEntity.Has<CurrentPosition>();
            var playerHasMoveSpeed = playerEntity.Has<MoveSpeed>();
            var playerInputExists = _world.Has<DownKey>() || _world.Has<UpKey>() || _world.Has<LeftKey>() || _world.Has<RightKey>();

            var playerHasVelocity = playerIsOnBoard && playerHasMoveSpeed && playerInputExists;

            var result = new QueryResult(
                QueryTypeEnum.PlayerIsMoving,
                true,
                playerHasVelocity,
                playerHasVelocity.GetType()
            );

            return Task.FromResult(result);
        }
    }
}