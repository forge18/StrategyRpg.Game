using Godot;

namespace Presentation.Services
{
    public interface INodeLocatorService
    {
        void AddNodeByEntityId(int entityId, Node node);
        void AddNodeByKey(string key, Node node);
        Node GetNodeByEntityId(int entityId);
        Node GetNodeByKey(string key);
        void RemoveNodeByKey(string key);
        bool HasNode(string key);
    }
}