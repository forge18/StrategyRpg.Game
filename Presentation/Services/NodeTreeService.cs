using Microsoft.Extensions.Logging;
using Godot;
using System.Collections.Generic;

namespace Presentation.Services
{
    public class NodeTreeService : INodeTreeService
    {
        private static Dictionary<NodeKeyEnum, Node> _nodeByKey = new Dictionary<NodeKeyEnum, Node>();
        private static Dictionary<int, Node> _nodeByEntityId = new Dictionary<int, Node>();

        private readonly ILogger<NodeTreeService> _logger;
        private Node _rootNode;

        public NodeTreeService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NodeTreeService>();
            _rootNode = GetNode(NodeKeyEnum.Root);
        }

        public Node CreateNode(NodeKeyEnum key)
        {
            if (HasNode(key))
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
            if (!_nodeByKey.ContainsKey(key))
            {
                _logger.LogWarning($"NodeLocatorService.GetNodeByKey: Node with key {key.ToString()} does not exist.");
                return default;
            }

            return _nodeByKey[key];
        }

        public Node GetNode(int entityId)
        {
            if (!_nodeByEntityId.ContainsKey(entityId))
            {
                _logger.LogWarning($"NodeLocatorService.GetNodeByEntityId: Node with entityId {entityId} does not exist.");
                return default;
            }

            return _nodeByEntityId[entityId];
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
            AddNodeToLookupByKey(key, newNode);

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
            RemoveNodeFromLookupByKey(key);

            node.QueueFree();
        }

        public void AddNodeToLookupByEntityId(int entityId, Node node)
        {
            if (_nodeByEntityId.ContainsKey(entityId))
            {
                _logger.LogWarning($"NodeLocatorService.AddNodeByEntityId: Node with entityId {entityId} already exists.");
                return;
            }

            _nodeByEntityId.Add(entityId, node);
        }

        public void AddNodeToLookupByKey(NodeKeyEnum key, Node node)
        {
            if (_nodeByKey.ContainsKey(key))
            {
                _logger.LogWarning($"NodeLocatorService.AddNodeByKey: Node with key {key.ToString()} already exists.");
                return;
            }

            _nodeByKey.Add(key, node);
        }

        public void RemoveNodeFromLookupByKey(NodeKeyEnum key)
        {
            if (!_nodeByKey.ContainsKey(key))
            {
                _logger.LogWarning($"NodeLocatorService.RemoveNodeByKey: Node with key {key.ToString()} does not exist.");
                return;
            }

            _nodeByKey.Remove(key);
        }

        public bool HasNode(NodeKeyEnum key)
        {
            return _nodeByKey.ContainsKey(key);
        }
    }


}