using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Godot;

namespace Presentation.Services
{
    public class NodeLocatorService : INodeLocatorService
    {
        private readonly ILogger<NodeLocatorService> _logger;

        private static Dictionary<NodeKeyEnum, Node> _nodeByKey = new Dictionary<NodeKeyEnum, Node>();
        private static Dictionary<int, Node> _nodeByEntityId = new Dictionary<int, Node>();

        public NodeLocatorService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NodeLocatorService>();
        }

        public void AddNodeByEntityId(int entityId, Node node)
        {
            if (_nodeByEntityId.ContainsKey(entityId))
            {
                _logger.LogWarning($"NodeLocatorService.AddNodeByEntityId: Node with entityId {entityId} already exists.");
                return;
            }

            _nodeByEntityId.Add(entityId, node);
        }

        public void AddNodeByKey(NodeKeyEnum key, Node node)
        {
            if (_nodeByKey.ContainsKey(key))
            {
                _logger.LogWarning($"NodeLocatorService.AddNodeByKey: Node with key {key.ToString()} already exists.");
                return;
            }

            _nodeByKey.Add(key, node);
        }

        public Node GetNodeByEntityId(int entityId)
        {
            if (!_nodeByEntityId.ContainsKey(entityId))
            {
                _logger.LogWarning($"NodeLocatorService.GetNodeByEntityId: Node with entityId {entityId} does not exist.");
                return default;
            }

            return _nodeByEntityId[entityId];
        }

        public Node GetNodeByKey(NodeKeyEnum key)
        {
            if (!_nodeByKey.ContainsKey(key))
            {
                _logger.LogWarning($"NodeLocatorService.GetNodeByKey: Node with key {key.ToString()} does not exist.");
                return default;
            }

            return _nodeByKey[key];
        }

        public void RemoveNodeByKey(NodeKeyEnum key)
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