using Microsoft.Extensions.Logging;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Presentation.Nodes;
using Presentation.Services;

namespace Infrastructure
{
    public class Bootstrapper
    {
        private const string _gameScriptPath = "res://Presentation/Nodes/Game.cs";
        private const string _inputScriptPath = "res://Presentation/Nodes/Input.cs";

        private readonly IMediator _mediator;
        private readonly IEcsSystemService _ecsSystemService;
        private readonly INodeLocatorService _nodeLocatorService;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<Bootstrapper> _logger;

        private Node _gameRootNode;

        public Bootstrapper(
            IMediator mediator, 
            IEcsSystemService ecsSystemService, 
            INodeLocatorService nodeLocatorService, 
            ILoggerFactory loggerFactory,
            Node gameRootNode
        )
        {
            _mediator = mediator;
            _ecsSystemService = ecsSystemService;
            _nodeLocatorService = nodeLocatorService;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Bootstrapper>();
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
            CreateUnitsNode();
        }

        public void CreateGameNode()
        {
            var gameNode = new Game(_mediator, _ecsSystemService, _loggerFactory);
            gameNode.Name = NodeKeyEnum.Game.ToString();

            _gameRootNode.AddChild(gameNode);
            _nodeLocatorService.AddNodeByKey(NodeKeyEnum.Game, gameNode);
        }

        public void CreateInputNode()
        {
            var inputNode = new PlayerInput(_mediator);
            inputNode.Name = NodeKeyEnum.PlayerInput.ToString();

            _gameRootNode.AddChild(inputNode);
            _nodeLocatorService.AddNodeByKey(NodeKeyEnum.PlayerInput, inputNode);
        }

        public void CreateUnitsNode()
        {
            var unitsNode = new Node();
            unitsNode.Name = NodeKeyEnum.Units.ToString();

            _gameRootNode.AddChild(unitsNode);
            _nodeLocatorService.AddNodeByKey(NodeKeyEnum.Units, unitsNode);
        }
    }
}