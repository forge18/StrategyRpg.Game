using Godot;
using Infrastructure.MediatorNS;
using Presentation.Nodes;
using Presentation.Services;

namespace Infrastructure
{
    public class Bootstrapper
    {
        private const string _gameScriptPath = "res://Presentation/Nodes/Game.cs";
        private const string _inputScriptPath = "res://Presentation/Nodes/Input.cs";

        private readonly IMediator _mediator;
        private readonly INodeLocatorService _nodeLocatorService;

        private Node _gameRootNode;

        public Bootstrapper(IMediator mediator, INodeLocatorService nodeLocatorService, Node gameRootNode)
        {
            _mediator = mediator;
            _nodeLocatorService = nodeLocatorService;
            _gameRootNode = gameRootNode;
        }

        public void Run()
        {
            CreateInitialNodeTree();
        }

        public void CreateInitialNodeTree()
        {
            CreateGameNode();
            CreateInputNode();
        }

        public void CreateGameNode()
        {
            var gameNode = new Game(_mediator);
            gameNode.Name = "Game";

            _gameRootNode.AddChild(gameNode);
            _nodeLocatorService.AddNodeByKey("Game", gameNode);
        }

        public void CreateInputNode()
        {
            var inputNode = new PlayerInput(_mediator);
            inputNode.Name = "PlayerInput";

            _gameRootNode.AddChild(inputNode);
            _nodeLocatorService.AddNodeByKey("PlayerInput", inputNode);
        }

        public void CreateUnitsNode()
        {
            var unitsNode = new Node();
            unitsNode.Name = "Units";

            _gameRootNode.AddChild(unitsNode);
            _nodeLocatorService.AddNodeByKey("Units", unitsNode);
        }
    }
}