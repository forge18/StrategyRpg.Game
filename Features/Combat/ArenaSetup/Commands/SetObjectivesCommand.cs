using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Infrastructure.Hub;

namespace Features.Combat.ArenaSetup
{
    public class SetObjectivesCommand : ICommand
    {
        public readonly World Arena;
        public readonly string[] Objectives;

        public SetObjectivesCommand(IEcsWorldService ecsWorldService, string[] objectives)
        {
            Arena = ecsWorldService.GetWorld("Arena");
            Objectives = objectives;
        }
    }

    public class SetObjectivesHandler : ICommandHandler<SetObjectivesCommand>, IHasEnum
    {
        public int GetEnum()
        {
            return (int)CommandTypeEnum.SetObjectives;
        }

        public Task Handle(SetObjectivesCommand command, CancellationToken cancellationToken = default)
        {
            command.Arena.Set<Objectives>(new Objectives(){ Values = command.Objectives });

            return Task.CompletedTask;
        }
    }
}