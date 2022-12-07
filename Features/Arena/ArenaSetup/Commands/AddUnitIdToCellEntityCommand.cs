using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Hub;

namespace Features.Arena.ArenaSetup
{
    public class AddUnitIdToCellEntityCommand : ICommand
    {
        public Entity CellEntity { get; set; }

        public Entity UnitEntity { get; set; }

        public AddUnitIdToCellEntityCommand(
            Entity cellEntity,
            Entity unitEntity
        )
        {
            CellEntity = cellEntity;
            UnitEntity = unitEntity;
        }
    }

    public class AddUnitIdToCellEntityHandler : ICommandHandler<AddUnitIdToCellEntityCommand>, IHasEnum
    {
        public readonly IEcsEntityService _ecsEntityService;

        public AddUnitIdToCellEntityHandler(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.AddUnitIdToCellEntity;
        }

        public Task Handle(AddUnitIdToCellEntityCommand command, CancellationToken cancellationToken = default)
        {
            if (command.CellEntity.Has<UnitInCell>())
            {
                GD.Print("Cell already has unit");
                return default;
            }

            command.CellEntity.Set<UnitInCell>(
                new UnitInCell(){
                    UnitId = _ecsEntityService.ParseEntityId(command.UnitEntity) 
                }
            );

            return Task.CompletedTask;
        }
    }
}