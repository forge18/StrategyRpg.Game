using DefaultEcs;
using Godot;

namespace Infrastructure.Map
{
    public interface IMapService
    {
        void LoadMapEntityIntoEcs(MapEnum mapName);
        Entity GetMapEntityFromEcs(MapEnum map);
        PackedScene GetMapScene(MapEnum map);
        void LoadMapSceneIntoNodeTree(PackedScene mapScene, Entity mapEntity);
    }
}