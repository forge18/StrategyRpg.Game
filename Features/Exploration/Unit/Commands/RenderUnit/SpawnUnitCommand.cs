using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Presentation.Services;

namespace Features.Exploration.Unit
{
    public class SpawnUnitCommand : ICommand
    {
        public readonly IMediator mediator;
        public readonly INodeLocatorService nodeLocatorService;
        public Guid ProcessId { get; set; }
        public Entity UnitEntity { get; set; }
        public Entity UnitTypeEntity { get; set; }

        public SpawnUnitCommand(
            IMediator mediator, 
            INodeLocatorService nodeLocatorService, 
            Guid processId, 
            Entity unitEntity, 
            Entity unitTypeEntity
        )
        {
            this.mediator = mediator;
            this.nodeLocatorService = nodeLocatorService;

            ProcessId = processId;
            UnitEntity = unitEntity;
            UnitTypeEntity = unitTypeEntity;
        }
    }

    public class SpawnUnitHandler : ICommandHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public SpawnUnitHandler(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public CommandTypeEnum GetEnum()
        {
            return CommandTypeEnum.SpawnUnit;
        }

        public Task Handle(ICommand genericCommand, CancellationToken cancellationToken = default)
        {
            var command = genericCommand as SpawnUnitCommand;
            var position = command.UnitEntity.Get<CurrentPosition>().Value;

            var node = CreateUnitNode(command);
            node = AttachSpriteToUnit(command, node);
            AttachUnitToUnitsNode(command, node);

            return Task.CompletedTask;
        }

        public CharacterBody2D CreateUnitNode(SpawnUnitCommand command)
        {
            var node = ActivatorUtilities.CreateInstance<Presentation.Nodes.Units.Unit>(_serviceProvider);
            var position = command.UnitEntity.Get<CurrentPosition>().Value;
            node.Position = position;

            return node;
        }

        public CharacterBody2D AttachSpriteToUnit(SpawnUnitCommand command, CharacterBody2D node)
        {
            var spriteFilepath = command.UnitTypeEntity.Get<Sprite>().Filepath;
            var spriteNode = new Sprite2D();
            spriteNode.Texture = (Texture2D)GD.Load(spriteFilepath);
            node.AddChild(spriteNode);

            return node;
        }

        public void AttachUnitToUnitsNode(SpawnUnitCommand command, CharacterBody2D node)
        {
            var unitsNode = command.nodeLocatorService.GetNodeByKey("Units");
            if (unitsNode == null)
            {
                GD.PrintErr("Units node not found");
                return;
            }

            unitsNode.AddChild(node);

            command.nodeLocatorService.AddNodeByEntityId(command.UnitEntity.Get<EntityId>().Value, node);
        }
    }
}