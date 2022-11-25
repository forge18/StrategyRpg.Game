

using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;
using Infrastructure.MediatorNS.CommandManagement;
using Presentation.Services;

namespace Features.Exploration.Unit.Commands.MovePlayer
{
    public class MovePlayerCommand : ICommand
    {
        public Entity PlayerEntity { get; set; }
        public Vector2 Velocity { get; set; }

        public readonly INodeLocatorService nodeLocatorService;
        public readonly IEcsEntityService ecsEntityService;

        public MovePlayerCommand(INodeLocatorService nodeLocatorService, IEcsEntityService ecsEntityService)
        {
            this.nodeLocatorService = nodeLocatorService;
            this.ecsEntityService = ecsEntityService;
        }
    }

    public class MovePlayerHandler : ICommandHandler<MovePlayerCommand>
    {
        public Task Handle(MovePlayerCommand command, CancellationToken cancellationToken = default)
        {
            var entityId = command.ecsEntityService.ParseEntityId(command.PlayerEntity);

            CharacterBody2D body = (CharacterBody2D)command.nodeLocatorService.GetNodeByEntityId(entityId);
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