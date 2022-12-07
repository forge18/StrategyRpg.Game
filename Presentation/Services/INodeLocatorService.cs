using Godot;

namespace Presentation.Services
{
    public interface INodeLocatorService
    {
        void AddNodeByEntityId(int entityId, Node node);
        void AddNodeByKey(NodeKeyEnum key, Node node);
        Node GetNodeByEntityId(int entityId);
        Node GetNodeByKey(NodeKeyEnum key);
        void RemoveNodeByKey(NodeKeyEnum key);
        bool HasNode(NodeKeyEnum key);
    }
}