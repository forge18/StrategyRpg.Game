using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;

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

    public class SetObjectivesHandler : ICommandHandler
    {
        public CommandTypeEnum GetEnum()
        {
            return CommandTypeEnum.SetObjectives;
        }

        public Task Handle(ICommand genericCommand, CancellationToken cancellationToken = default)
        {
            var command = genericCommand as SetObjectivesCommand;

            command.Arena.Set<Objectives>(new Objectives(){ Values = command.Objectives });

            return Task.CompletedTask;
        }
    }
}