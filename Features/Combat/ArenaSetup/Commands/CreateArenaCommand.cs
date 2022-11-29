using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.HubMediator;
using Presentation.Services;

namespace Features.Combat.ArenaSetup
{
    public class CreateArenaCommand : ICommand
    {
        public readonly World Arena;

        public readonly INodeService _nodeService;

        public CreateArenaCommand(World world, INodeService nodeService)
        {
            Arena = world;
            _nodeService = nodeService;
        }
    }

    public class CreateArenaHandler : ICommandHandler
    {
        public CommandTypeEnum GetEnum()
        {
            return CommandTypeEnum.CreateArena;
        }

        public Task Handle(ICommand genericCommand, CancellationToken cancellationToken = default)
        {
            var command = genericCommand as CreateArenaCommand;

            var arenaNode = command._nodeService.CreateNode("Arena");
            var gameNode = command._nodeService.GetNode("Game");
            command._nodeService.AddNodeToTree(arenaNode, gameNode);

            return Task.CompletedTask;
        }
    }
}