using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Hub;

namespace Features.Arena.ArenaSetup
{
    public class SetObjectivesCommand : ICommand
    {
        public readonly string[] Objectives;

        public SetObjectivesCommand(IEcsWorldService ecsWorldService, string[] objectives)
        {
            Objectives = objectives;
        }
    }

    public class SetObjectivesHandler : ICommandHandler<SetObjectivesCommand>, IHasEnum
    {
        private readonly World _arena;

        public SetObjectivesHandler(IEcsWorldService ecsWorldService)
        {
            _arena = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.SetObjectives;
        }

        public Task Handle(SetObjectivesCommand command, CancellationToken cancellationToken = default)
        {
            _arena.Set<Objectives>(new Objectives(){ Values = command.Objectives });

            return Task.CompletedTask;
        }
    }
}