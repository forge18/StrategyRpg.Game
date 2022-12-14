using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Presentation.Services;

namespace Modules.Exploration
{
    public class MovePlayerCommand : ICommand
    {
        public Entity PlayerEntity { get; set; }
        public Vector2 Velocity { get; set; }

        public MovePlayerCommand(DefaultEcs.Entity playerEntity, Vector2 velocity)
        {
            PlayerEntity = playerEntity;
            Velocity = velocity;
        }
    }

    public class MovePlayerHandler : ICommandHandler<MovePlayerCommand>, IHasEnum
    {
        private readonly INodeTreeService _nodeTreeService;
        private readonly IEcsEntityService _ecsEntityService;

        public MovePlayerHandler(
            INodeTreeService nodeTreeService,
            IEcsEntityService ecsEntityService
        )
        {
            _nodeTreeService = nodeTreeService;
            _ecsEntityService = ecsEntityService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.MovePlayer;
        }

        public Task Handle(MovePlayerCommand command, CancellationToken cancellationToken = default)
        {

            var entityId = _ecsEntityService.ParseEntityId(command.PlayerEntity);

            CharacterBody2D body = (CharacterBody2D)_nodeTreeService.GetNode(entityId);
            var speed = command.PlayerEntity.Get<MoveSpeed>().Value;
            var moveVelocity = command.Velocity * speed;

            var collision = body.MoveAndCollide(moveVelocity);
            if (collision != null)
            {
                GD.Print(body.GetInstanceId(), " has collided with ", ((Node)collision.GetCollider()).Name);
            }

            command.PlayerEntity.Remove<Velocity>();
            return Task.CompletedTask;
        }
    }
}