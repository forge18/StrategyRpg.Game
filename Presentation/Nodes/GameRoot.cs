using Godot;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Presentation.Nodes
{
    public partial class GameRoot : Node
    {
        private readonly ILogger<GameRoot> _logger;

        private GameRoot() { }
        public GameRoot(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GameRoot>();

            new Startup(this).Run();
            _logger.LogInformation("GameRoot: GameRoot initialized.");
        }
    }
}