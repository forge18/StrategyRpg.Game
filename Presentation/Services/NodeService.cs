using Microsoft.Extensions.Logging;
using Godot;

namespace Presentation.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeLocatorService _nodeLocatorService;
        private readonly ILogger<NodeService> _logger;
        private Node _rootNode;

        public NodeService(INodeLocatorService nodeLocatorService, ILoggerFactory loggerFactory)
        {
            _nodeLocatorService = nodeLocatorService;
            _logger = loggerFactory.CreateLogger<NodeService>();
            _rootNode = _nodeLocatorService.GetNodeByKey(NodeKeyEnum.Root);
        }

        public Node CreateNode(NodeKeyEnum key)
        {
            if (_nodeLocatorService.HasNode(key))
            {
                _logger.LogWarning($"NodeService.CreateNode: Node with name {key.ToString()} already exists.");
                return default;
            };

            var node = new Node();
            node.Name = key.ToString();

            return node;
        }

        public Node GetNode(NodePath nodePath)
        {
            var node = _rootNode.GetNode(nodePath);
            if (node == null)
            {
                _logger.LogWarning($"NodeService.GetNode: Node with path {nodePath} does not exist.");
                return default;
            }

            return node;
        }

        public Node GetNode(NodeKeyEnum key)
        {
            var node = _nodeLocatorService.GetNodeByKey(key);
            if (node == null)
            {
                _logger.LogWarning($"NodeService.GetNode: Node with key {key.ToString()} does not exist.");
                return default;
            }

            return node;
        }

        public Node GetNode(int entityId)
        {
            var node = _nodeLocatorService.GetNodeByEntityId(entityId);
            if (node == null)
            {
                _logger.LogWarning($"NodeService.GetNode: Node with EntityId {entityId} does not exist.");
                return default;
            }
            return node;
        }

        public Node AddNodeToTree(Node newNode, Node parentNode = null)
        {
            if (parentNode != null)
            {
                _rootNode.AddChild(newNode);
            }
            else
            {
                parentNode.AddChild(newNode);
            }

            var key = (NodeKeyEnum)System.Enum.Parse(typeof(NodeKeyEnum), newNode.Name);
            _nodeLocatorService.AddNodeByKey(key, newNode);

            return newNode;
        }

        public Node MoveNodeInTree(Node node, Node newParentNode)
        {
            var newNode = node.Duplicate();
            AddNodeToTree(newNode, newParentNode);
            RemoveNodeFromTree(node);

            return node;
        }

        public void RemoveNodeFromTree(Node node)
        {
            var key = (NodeKeyEnum)System.Enum.Parse(typeof(NodeKeyEnum), node.Name);
            _nodeLocatorService.RemoveNodeByKey(key);

            node.QueueFree();
        }
    }


}