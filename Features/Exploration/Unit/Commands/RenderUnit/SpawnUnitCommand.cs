using System;
using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.MediatorNS;
using Infrastructure.MediatorNS.CommandManagement;
using Infrastructure.MediatorNS.QueryManagement;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;

namespace StrategyRpg.Game.Features.Exploration.Unit.Commands.RenderUnit
{
    public class SpawnUnitCommand : IQuery
    {
        public readonly IMediator mediator;
        public readonly INodeLocatorService nodeLocatorService;
        public Guid ProcessId { get; set; }
        public Entity UnitEntity { get; set; }
        public Entity UnitTypeEntity { get; set; }

        public SpawnUnitCommand(IMediator mediator, INodeLocatorService nodeLocatorService, Guid processId, Entity unitEntity, Entity unitTypeEntity)
        {
            this.mediator = mediator;
            this.nodeLocatorService = nodeLocatorService;

            ProcessId = processId;
            UnitEntity = unitEntity;
            UnitTypeEntity = unitTypeEntity;
        }
    }

    public class SpawnUnitHandler : ICommandHandler<SpawnUnitCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public SpawnUnitHandler(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
        }

        public Task Handle(SpawnUnitCommand command, CancellationToken cancellationToken = default)
        {
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

            // command.nodeLocatorService.AddNodeByKey("Unit" + node.GetInstanceId(), node);
            // command.nodeLocatorService.AddNodeByEntityId.(command.UnitEntity.Get<EntityId>().Value, node);
        }
    }
}