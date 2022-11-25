using System;
using DefaultEcs;
using Features.Exploration.Unit.Commands.MovePlayer;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Worlds;
using Infrastructure.MediatorNS;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.QueryManagement;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;

namespace Features.Exploration.Unit.Watchers
{
    public class PlayerVelocityWatcher : Watcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly INodeLocatorService _nodeLocatorService;
        private readonly IEcsEntityService _ecsEntityService;

        private readonly World _world;
        private readonly Entity _playerEntity;

        public PlayerVelocityWatcher(
            IServiceProvider serviceProvider,
            IMediator mediator,
            INodeLocatorService nodeLocatorService,
            IEcsWorldService ecsWorldService,
            IEcsEntityService ecsEntityService
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _nodeLocatorService = nodeLocatorService;
            _ecsEntityService = ecsEntityService;

            _world = ecsWorldService.GetWorld();

            var result = _mediator.RunQuery(QueryTypeEnum.GetPlayerEntity);
            _playerEntity = result.ConvertResultValue<Entity>();
        }

        public override void Update(float elapsedTime)
        {
            var playerIsMovingResult = _mediator.RunQuery(QueryTypeEnum.PlayerIsMoving);
            if (!playerIsMovingResult.ConvertResultValue<bool>())
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

            var commandData = ActivatorUtilities.CreateInstance<MovePlayerCommand>(_serviceProvider);
            commandData.PlayerEntity = _playerEntity;
            commandData.Velocity = velocity;

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