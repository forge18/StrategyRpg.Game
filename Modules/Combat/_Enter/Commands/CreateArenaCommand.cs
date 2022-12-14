using System.Threading;
using System.Threading.Tasks;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Presentation.Services;

namespace Modules.Combat
{
    public class CreateArenaCommand : ICommand
    {

    }

    public class CreateArenaHandler : ICommandHandler<CreateArenaCommand>, IHasEnum
    {
        public readonly World Arena;

        public readonly INodeTreeService _nodeTreeService;

        public CreateArenaHandler(IEcsWorldService ecsWorldService, INodeTreeService nodeTreeService)
        {
            Arena = ecsWorldService.GetWorld(EcsWorldEnum.Arena);
            _nodeTreeService = nodeTreeService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.CreateArena;
        }

        public Task Handle(CreateArenaCommand command, CancellationToken cancellationToken = default)
        {
            var arenaNode = _nodeTreeService.CreateNode(NodeKeyEnum.Arena);
            var gameNode = _nodeTreeService.GetNode(NodeKeyEnum.Game);
            _nodeTreeService.AddNodeToTree(arenaNode, gameNode);

            return Task.CompletedTask;
        }
    }
}