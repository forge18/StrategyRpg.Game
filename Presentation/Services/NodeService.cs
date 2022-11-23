using Godot;
using Presentation.Interfaces;

namespace Presentation.Services
{
    public class NodeService : INodeService
    {
        private readonly NodeLocatorService _nodeLocatorService;
        private Node _rootNode;

        public NodeService(NodeLocatorService nodeLocatorService)
        {
            _nodeLocatorService = nodeLocatorService;
            _rootNode = _nodeLocatorService.GetNodeByKey("root");
        }

        public TNode CreateNode<TNode>(string nodeName) where TNode : INode, new()
        {
            var test = new Node2D();
            var node = new TNode();
            node.Name = nodeName;
            return node;
        }

        public TNode CreateNodeFromScriptPath<TNode>(string scriptPath) where TNode : CSharpScript
        {
            TNode node = (TNode)ResourceLoader.Load<TNode>(scriptPath).New();
            return node;
        }

        public Node GetNode(NodePath nodePath)
        {
            var node = _rootNode.GetNode(nodePath);
            if (node == null)
            {
                GD.Print("NodeService: Node not found for node path: ", nodePath);
                return default;
            }

            return node;
        }

        public Node GetNode(string key)
        {
            var node = _nodeLocatorService.GetNodeByKey(key);
            if (node == null)
            {
                GD.Print("NodeService: Node not found for key: ", key);
                return default;
            }

            return node;
        }

        public Node GetNode(int entityId)
        {
            var node = _nodeLocatorService.GetNodeByEntityId(entityId);
            if (node == null)
            {
                GD.Print("NodeService: Node not found for EntityId: ", entityId);
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