using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Presentation.Services;

namespace Features.Arena.ArenaSetup
{
    public class CreateArenaCommand : ICommand
    {
        
    }

    public class CreateArenaHandler : ICommandHandler<CreateArenaCommand>, IHasEnum
    {
        public readonly World Arena;

        public readonly INodeService _nodeService;

        public CreateArenaHandler(World world, INodeService nodeService)
        {
            Arena = world;
            _nodeService = nodeService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.CreateArena;
        }

        public Task Handle(CreateArenaCommand command, CancellationToken cancellationToken = default)
        {
            var arenaNode = _nodeService.CreateNode(NodeKeyEnum.Arena);
            var gameNode = _nodeService.GetNode(NodeKeyEnum.Game);
            _nodeService.AddNodeToTree(arenaNode, gameNode);

            return Task.CompletedTask;
        }
    }
}