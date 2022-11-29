using Godot;
using Infrastructure.DependencyInjection;

namespace Presentation.Nodes
{
    public partial class GameRoot : Node
    {
        public GameRoot()
        {
            new Startup(this).Run();
        }
    }
}