using System;
using System.Collections.Generic;
using Godot;

namespace Presentation.Services
{
    public class NodeLocatorService : INodeLocatorService
    {
        private static Dictionary<string, Node> _nodeByKey = new Dictionary<string, Node>();
        private static Dictionary<int, Node> _nodeByEntityId = new Dictionary<int, Node>();

        public void AddNodeByEntityId(int entityId, Node node)
        {
            if (_nodeByEntityId.ContainsKey(entityId))
            {
                new Exception($"NodeService.AddNodeByEntityId: Node with EntityId {entityId} already exists.");
            }

            _nodeByEntityId.Add(entityId, node);
        }

        public void AddNodeByKey(string key, Node node)
        {
            if (_nodeByKey.ContainsKey(key))
            {
                new Exception($"NodeService.AddNodeByKey: Node with key {key} already exists.");
            }

            _nodeByKey.Add(key, node);
        }

        public Node GetNodeByEntityId(int entityId)
        {
            if (!_nodeByEntityId.ContainsKey(entityId))
            {
                new Exception($"NodeService.GetNodeByEntityId: Node with EntityId {entityId} does not exist.");
            }

            return _nodeByEntityId[entityId];
        }

        public Node GetNodeByKey(string key)
        {
            if (!_nodeByKey.ContainsKey(key))
            {
                new Exception($"NodeService.GetNodeReferenceByKey: Node with key {key} does not exist.");
            }

            return _nodeByKey[key];
        }

        public void RemoveNodeByKey(string key)
        {
            if (!_nodeByKey.ContainsKey(key))
            {
                new Exception($"NodeService.RemoveNodeByKey: Node with key {key} does not exist.");
            }

            _nodeByKey.Remove(key);
        }

        public bool HasNode(string key)
        {
            return _nodeByKey.ContainsKey(key);
        }
    }
}