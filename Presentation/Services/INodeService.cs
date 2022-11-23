using Godot;
using Presentation.Interfaces;

namespace Presentation.Services
{
    public interface INodeService
    {
        TNode CreateNode<TNode>(string nodeName) where TNode : INode, new();
        TNode CreateNodeFromScriptPath<TNode>(string scriptPath) where TNode : CSharpScript;

        Node GetNode(NodePath nodePath);
        Node GetNode(string key);
        Node GetNode(int entityId);

        Node AddNodeToTree(Node newNode, Node parentNode = null);
        Node MoveNodeInTree(Node node, Node newParentNode);
        void RemoveNodeFromTree(Node node);
    }
}