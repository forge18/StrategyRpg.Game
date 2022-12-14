using System;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Exploration
{
    public class PlayerVelocitySystem : EcsSystem
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        private readonly World _world;
        private readonly Entity _playerEntity;

        public PlayerVelocitySystem(
            IServiceProvider serviceProvider,
            IMediator mediator,
            IEcsWorldService ecsWorldService
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;

            _world = ecsWorldService.GetWorld();

            var result = _mediator.RunQuery(QueryTypeEnum.GetPlayerEntity);
            _playerEntity = result.Result.ConvertResultValue<DefaultEcs.Entity>();
        }

        public override void Update(float elapsedTime)
        {
            var playerIsMovingResult = _mediator.RunQuery(QueryTypeEnum.PlayerIsMoving);
            if (!playerIsMovingResult.Result.ConvertResultValue<bool>())
            {
                return;
            }

            if (!_playerEntity.Has<Velocity>())
            {
                _playerEntity.Set<Velocity>(new Velocity { Value = new Vector2(0, 0) });
            }

            ref Velocity playerVelocity = ref _playerEntity.Get<Velocity>();

            var velocity = GetVector2FromInput();
            _playerEntity.Set<IsMoving>();

            var commandData = ActivatorUtilities.CreateInstance<MovePlayerCommand>(_serviceProvider, new object[] {
                _playerEntity,
                velocity
            });

            _mediator.ExecuteCommand(CommandTypeEnum.MovePlayer, commandData);
        }

        private Vector2 GetVector2FromInput()
        {
            Vector2 velocity = new Vector2();
            if (_world.Has<RightKey>())
                velocity.x += 1;
            if (_world.Has<LeftKey>())
                velocity.x -= 1;
            if (_world.Has<DownKey>())
                velocity.y += 1;
            if (_world.Has<UpKey>())
                velocity.y -= 1;
            return velocity.Normalized();
        }
    }
}