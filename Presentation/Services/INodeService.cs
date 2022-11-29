using Godot;

namespace Presentation.Services
{
    public interface INodeService
    {
        Node CreateNode(string nodeName);

        Node GetNode(NodePath nodePath);
        Node GetNode(string key);
        Node GetNode(int entityId);

        Node AddNodeToTree(Node newNode, Node parentNode = null);
        Node MoveNodeInTree(Node node, Node newParentNode);
        void RemoveNodeFromTree(Node node);
    }
}