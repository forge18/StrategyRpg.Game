using Godot;

namespace Presentation.Services
{
    public interface INodeTreeService
    {
        Node CreateNode(NodeKeyEnum key);

        Node GetNode(NodePath nodePath);
        Node GetNode(NodeKeyEnum key);
        Node GetNode(int entityId);

        Node AddNodeToTree(Node newNode, Node parentNode = null);
        Node MoveNodeInTree(Node node, Node newParentNode);
        void RemoveNodeFromTree(Node node);

        void AddNodeToLookupByEntityId(int entityId, Node node);
        void AddNodeToLookupByKey(NodeKeyEnum key, Node node);
        void RemoveNodeFromLookupByKey(NodeKeyEnum key);
        bool HasNode(NodeKeyEnum key);
    }
}