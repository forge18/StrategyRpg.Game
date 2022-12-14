using System.Threading;
using System.Threading.Tasks;
using Godot;
using Infrastructure.Hub;
using Infrastructure.HubMediator;
using Infrastructure.Map;
using Presentation.Services;

namespace Modules.Combat
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
        public readonly INodeTreeService _nodeTreeService;

        public LoadArenaMapHandler(
            INodeTreeService nodeTreeService
        )
        {
            _nodeTreeService = nodeTreeService;
        }

        public int GetEnum()
        {
            return (int)CommandTypeEnum.LoadArenaMap;
        }

        public Task Handle(LoadArenaMapCommand command, CancellationToken cancellationToken = default)
        {
            var mapNode = ResourceLoader.Load<PackedScene>(command.MapScenePath).Instantiate();

            _nodeTreeService.AddNodeToTree(mapNode, _nodeTreeService.GetNode(NodeKeyEnum.Arena));

            var nodeKeyEnum = ConvertMapEnumToNodeKeyEnum(command.MapEnum);
            _nodeTreeService.AddNodeToLookupByKey(
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