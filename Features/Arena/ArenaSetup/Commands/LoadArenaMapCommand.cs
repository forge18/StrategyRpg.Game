using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Infrastructure.Map;
using Presentation.Services;

namespace Features.Arena.ArenaSetup
{
    public class LoadArenaMapCommand : ICommand
    {
        public MapEnum MapEnum { get; set; }
        public string MapScenePath { get; set; }

        public LoadArenaMapCommand(
            string mapScenePath
        )
        {
            MapScenePath = mapScenePath;
        }
    }

    public class LoadArenaMapHandler : ICommandHandler<LoadArenaMapCommand>, IHasEnum
    {
        public readonly INodeService _nodeService;
        public readonly INodeLocatorService _nodeLocatorService;

        public LoadArenaMapHandler(
            INodeService nodeService,
            INodeLocatorService nodeLocatorService
        )
        {
            _nodeService = nodeService;
            _nodeLocatorService = nodeLocatorService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.LoadArenaMap;
        }

        public Task Handle(LoadArenaMapCommand command, CancellationToken cancellationToken = default)
        {
            var mapNode = ResourceLoader.Load<PackedScene>(command.MapScenePath).Instantiate();

            _nodeService.AddNodeToTree(mapNode, _nodeLocatorService.GetNodeByKey(NodeKeyEnum.Arena));

            var nodeKeyEnum = ConvertMapEnumToNodeKeyEnum(command.MapEnum);
            _nodeLocatorService.AddNodeByKey(
                nodeKeyEnum,
                mapNode
            );

            return Task.CompletedTask;
        }

        private NodeKeyEnum ConvertMapEnumToNodeKeyEnum(MapEnum mapEnum)
        {
            var mapString = "Map" + mapEnum.ToString();
            return (NodeKeyEnum)System.Enum.Parse(typeof(NodeKeyEnum), mapString);
        }
    }
}