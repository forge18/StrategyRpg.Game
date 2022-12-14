using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Hub;
using Infrastructure.HubMediator;

namespace Modules.Combat
{
    public class RemoveUnitFromGridCommand: ICommand
    {
        public Entity CellEntity { get; set; }
        public Entity UnitEntity { get; set; }

        public RemoveUnitFromGridCommand(
            Entity cellEntity,
            Entity unitEntity
        )
        {
            CellEntity = cellEntity;
            UnitEntity = unitEntity;
        }
    }

    public class RemoveUnitFromGridHandler : ICommandHandler<RemoveUnitFromGridCommand>, IHasEnum
    {
        public int GetEnum()
        {
            return (int)CommandTypeEnum.RemoveUnitFromGrid;
        }

        public Task Handle(RemoveUnitFromGridCommand command, CancellationToken cancellationToken = default)
        {
            if (!command.CellEntity.Has<UnitInCell>())
            {
                GD.Print("Cell does not contain unit");
                return default;
            }

            command.CellEntity.Remove<UnitInCell>();

            return Task.CompletedTask;
        }
    }
}