using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Infrastructure.Hub;

namespace Features.Unit
{
    public class PlayerIsMovingQuery : IQuery
    {

    }

    public class PlayerIsMovingHandler : IQueryHandler<PlayerIsMovingQuery>, IHasEnum
    {
        private readonly World _world;
        private readonly ILogger<PlayerIsMovingHandler> _logger;

        public PlayerIsMovingHandler(IEcsWorldService ecsWorldService, ILoggerFactory loggerFactory) 
        {
            _world = ecsWorldService.GetWorld();
            _logger = loggerFactory.CreateLogger<PlayerIsMovingHandler>();
        }

        public int GetEnum()
        {
            return (int)QueryTypeEnum.PlayerIsMoving;
        }

        public Task<QueryResult> Handle(PlayerIsMovingQuery query, CancellationToken cancellationToken = default)
        {
            var playerEntity = _world.GetEntities().With<IsPlayerEntity>().AsSet().GetEntities()[0];

            var playerHasVelocity = PlayerHasVelocity(playerEntity);

            var result = new QueryResult(
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