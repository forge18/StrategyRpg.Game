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
            _rootNode = _nodeLocatorService.GetNodeByKey("root");
        }

        public Node CreateNode(string nodeName)
        {
            if (_nodeLocatorService.HasNode(nodeName))
            {
                _logger.LogWarning($"NodeService.CreateNode: Node with name {nodeName} already exists.");
                return default;
            };

            var node = new Node();
            node.Name = nodeName;

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

        public Node GetNode(string key)
        {
            var node = _nodeLocatorService.GetNodeByKey(key);
            if (node == null)
            {
                _logger.LogWarning($"NodeService.GetNode: Node with key {key} does not exist.");
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

            _nodeLocatorService.AddNodeByKey(newNode.Name, newNode);

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
            _nodeLocatorService.RemoveNodeByKey(node.Name);

            node.QueueFree();
        }
    }


}