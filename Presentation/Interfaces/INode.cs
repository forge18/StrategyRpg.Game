using Godot;
using static Godot.Node;

namespace Presentation.Interfaces
{
    public interface INode
    {
        public string EditorDescription { get; set; }
        public MultiplayerAPI Multiplayer { get; set; }
        public string Name { get; set; }
        public Node Owner { get; set; }
        public ProcessModeEnum ProcessMode { get; set; }
        public int ProcessPriority { get; set; }
        public string SceneFilePath { get; set; }
        public bool UniqueNameInOwner { get; set; }
    }
}